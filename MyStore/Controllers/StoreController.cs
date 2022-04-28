using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using MyStore.Models.Home.BindingModels;
using MyStore.Models.Store;
using MyStore.Infrastructure;
//using MyStore.Models.StoreEntities;

namespace MyStore.Controllers
{
    [LogTime]
    public class StoreController : Controller
    {
        private readonly IODatabase _iODatabase;
        private readonly IOStore _iOStore;
        private readonly CartService _cartService;

        public StoreController(IODatabase iODatabase, IOStore iOStore, CartService cartService)
        {
            _iODatabase = iODatabase;
            _iOStore = iOStore ;
            _cartService = cartService;
        }

        public async Task<IActionResult> Products(string section, string category)
        {
            StoreLayoutBindingModel storeLayoutBindingModel = new StoreLayoutBindingModel();
            storeLayoutBindingModel.ColoredProducts = await _iODatabase.GetCatalogProducts(section, category);

            foreach (var coloredProduct in storeLayoutBindingModel.ColoredProducts)
            {
                coloredProduct.CartQuantity = 1;
            }


            return View(storeLayoutBindingModel);
        }
        public async Task<IActionResult> Product(int ProductId, int ColorId)
        {
            StoreLayoutBindingModel storeLayoutBindingModel = new StoreLayoutBindingModel();
            storeLayoutBindingModel.ColoredProduct = await _iODatabase.GetColoredProduct(ProductId, ColorId);
            storeLayoutBindingModel.ColoredProduct.CartQuantity = 1;

            return View(storeLayoutBindingModel);
        }

        //Сюда попадаем из JS по кнопкам "Купить"
        [HttpPost]
        public IActionResult AddToCart(StoreLayoutBindingModel storeLayoutBindingModel)
        {
            storeLayoutBindingModel.Message = _cartService.GetProductToCart(this.HttpContext, storeLayoutBindingModel.ColoredProduct);

            return Json(storeLayoutBindingModel.Message);
        }

        //Сюда переходим по ссылке
        [HttpPost]
        public IActionResult Cart()
        {
            var authorizedClient = HttpContext.Session.Get<Client>("authorizedClient");

            if ((authorizedClient != null) && (authorizedClient.Order != null) && (authorizedClient.Order.Purchases != null) && (authorizedClient.Order.Purchases.Count != 0))
            {

                return /*RedirectToAction("CartForm"),*/ // JS скрипт перенаправит в CartForm
                 Json(new
                 {
                     Message = "Корзина НЕ пуста"
                 });
            }
            else
            {
                return // JS скрипт просто выдаст сообщение
                 Json(new
                {
                    Message = "Корзина пуста"
                });
            }
        }

        //Первое открытие корзины
        public async Task<IActionResult> CartForm()
        {
            StoreLayoutBindingModel storeLayoutBindingModel = new StoreLayoutBindingModel();
            storeLayoutBindingModel.Client = HttpContext.Session.Get<Client>("authorizedClient");
            storeLayoutBindingModel.DeliveryTypesModel = await _iOStore.GetDeliveryTypesModel();

            return View(storeLayoutBindingModel);
        }

        //Уточняем данные перед полтверждением заказа
        [HttpPost]
        public async Task<IActionResult> CartForm(StoreLayoutBindingModel storeLayoutBindingModel)// сюда пришло только то,
                                                                                                  // что переприслалось из формы
        {
            var client = HttpContext.Session.Get<Client>("authorizedClient");

            storeLayoutBindingModel.Client.Order.Summ = client.Order.Summ;
            storeLayoutBindingModel.Client.RegisterDate = client.RegisterDate;
            storeLayoutBindingModel.Client.Email = client.Email;
            storeLayoutBindingModel.Client.PassHash = client.PassHash;

            HttpContext.Session.Set<Client>("authorizedClient", storeLayoutBindingModel.Client);
            storeLayoutBindingModel.DeliveryTypesModel = await _iOStore.GetDeliveryTypesModel();

           // storeLayoutBindingModel.StoreClient = SessionExtensions.ConvertToStoreClient(storeLayoutBindingModel.Client);

            return View(storeLayoutBindingModel);
        }

        //Отправляем заказ из сессии в БД
        public async Task<IActionResult> SendCartForm()
        {
            StoreLayoutBindingModel storeLayoutBindingModel = new StoreLayoutBindingModel();

            //получение содержимого корзины из сессии
            storeLayoutBindingModel.Client = HttpContext.Session.Get<Client>("authorizedClient");

            //отправка содержимого корзины в БД
            storeLayoutBindingModel.Message = await _iOStore.SendOrderToDB(storeLayoutBindingModel.Client);

            //очистка корзины
            HttpContext.Session.Set<Client>("authorizedClient", null); 

            return View(storeLayoutBindingModel);
        }
    }
}
