using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models.StoreEntities;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Infrastructure;
using MyStore.Models.Admin.BindingModels;
//using MyStore.Models.Admin.BindingModels;

namespace MyStore.Controllers
{
    [LogTime]
    [Authorization]
    public class AdminController : Controller
    {
        private readonly IODatabase _iODatabase;

        public AdminController(IODatabase iODatabase)
        {
            _iODatabase = iODatabase;
        }

        

        [HttpPost]
        public async Task<IActionResult> EditDescription(int productId, int colorId, string description)
        {
            await _iODatabase.SaveDescription(productId, description);

            var editProductBindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            return View("EditProduct", editProductBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditShowProduct(int productId, int colorId, bool showProduct)
        {
            await _iODatabase.SaveShowProduct(productId, colorId, showProduct);

            var editProductBindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            return View("EditProduct", editProductBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditStock(int productId, int colorId, int stock)
        {
            await _iODatabase.SaveStock(productId, colorId, stock);

            var editProductBindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            return View("EditProduct", editProductBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPrice(int productId, int colorId, decimal price)
        {
            await _iODatabase.SavePrice(productId, colorId, price);

            var editProductBindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            return View("EditProduct", editProductBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductCode(int productId, int colorId, string productCode)
        {
            await _iODatabase.SaveProductCode(productId, colorId, productCode);

            var editProductBindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            return View("EditProduct", editProductBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductName(int productId, int colorId, string productName)
        {
            await _iODatabase.SaveProductName(productId, colorId, productName);

            var editProductBindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            return View("EditProduct", editProductBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(string[] valueNames, int[] сategoryCharacteristicsIds, string[] inputValues, string[] toDeleteValueNames, EditProductBindingModel bindingModel)
        {
            List<string> Messages = new List<string>();

            //Добавление файла в БД
            if (ModelState["Files"].Errors.Count == 0)
            {
                 await _iODatabase.AddFilesToDatabase(bindingModel.Files, bindingModel.ProductId, bindingModel.ColorId);
            }
           
            //Удаление значения
            if (Array.Exists(toDeleteValueNames, element => element != null)) //Если в массиве есть хоть один элемент не null
            {
                
                Messages = _iODatabase.DeleteCharateristicValue(toDeleteValueNames, сategoryCharacteristicsIds);
            }

            //Добавление нового значения в характеристику категории
            if (Array.Exists(inputValues, element => element != null)) //Если в массиве есть хоть один элемент не null
            {
                Messages.AddRange(_iODatabase.AddCharacteristicValue(inputValues, сategoryCharacteristicsIds));
            }

            //Выбор другого значения в характеристике категории для товара
            Messages.AddRange(_iODatabase.AddValue(valueNames, сategoryCharacteristicsIds, bindingModel.ProductId));

            var options = await _iODatabase.GetOptionNames();
            

            //EditProductBindingModel bindingModel = new();
            bindingModel = await _iODatabase.GetColoredProductWithValues(bindingModel.ProductId, bindingModel.ColorId);

            bindingModel.Messages = Messages;

            return View(bindingModel);
        }

        public async Task<IActionResult> DeleteImage(int productId, int colorId, int imageId)
        {
            var messages = await _iODatabase.DelImage(productId, colorId, imageId);

            EditProductBindingModel bindingModel = new EditProductBindingModel();
            bindingModel = await _iODatabase.GetColoredProductWithValues(productId, colorId);

            bindingModel.Messages = messages;

            return View("EditProduct", bindingModel);
        }


        // У вас есть сущность Product сделайте контроллер который выводит список продуктов, удаляет и редактирует их.
        // этот класс станет меньше, убедт отдельный контроллер и группа представлений для работы с продуктами
        // такой же подход для работы со всеми отсальными сущностями - катоегриям, комменатриями, заказами и т.д. иногда 
        // что то можно объеденить в один контроллер но точно не все сущности

        // вы можете сделать area Admin и в этой области сделать контроллеры ProcuctController, OrdersController, CategoriesContoller
        // у каждого контроллера будет представление Idex, Edit, Details, Delete (могут быть вариации в зависимости от спицифики сущности)
        
        // Практически во всех методах нет валидации, а админ может ошибиться и не заполнить обязательное поле или что то в этом духе
        // 
        public async Task<IActionResult> ProductsManagement()
        {
            //должны быть списки
            //список имен секций 
            //и категорий
            //передаём их с помощью BindingModel

            // BindingModel - это то что передается в параметы метода доступа - данные которые приходят в запросе и привязываются к параметрам
            // то что метод доступа передает в представление для отображения называется ViewModel

            ProductsManagementBindingModel bindingModel = new ProductsManagementBindingModel();
            var bindingModelOptions = await _iODatabase.GetOptionNames();

            var bindingModelProducts = await _iODatabase.GetFilteredCatalogProducts(true,"All", "All");

            // подобные действия присвоения одноименных свойств одного объекта в другой можно сделать через mapper библиотеку
            bindingModel.OptionCategoryList = bindingModelOptions.OptionCategoryList;
            bindingModel.OptionSectionList = bindingModelOptions.OptionSectionList;
            bindingModel.OptionColorList = bindingModelOptions.OptionColorList;

            bindingModel.ColoredProductsList = bindingModelProducts.ColoredProductsList;
            bindingModel.SelectedShowAll = true; 
            bindingModel.SelectedSection = "All";
            bindingModel.SelectedCategory = "All";
            bindingModel.SelectedSectionCreateProductForm = "All";
            bindingModel.SelectedCategoryCreateProductForm = "All";
            bindingModel.SelectedColorCreateProductForm = "-";

            return View(bindingModel);
        }

        

        [HttpPost]
        public async Task<IActionResult> ProductsManagement(ProductsManagementBindingModel bm)
        {
            var bindingModelOptions = await _iODatabase.GetOptionNames();

            var bindingModelProducts = await _iODatabase.GetFilteredCatalogProducts(bm.SelectedShowAll, bm.SelectedSection, bm.SelectedCategory);

            bm.OptionCategoryList = bindingModelOptions.OptionCategoryList;
            bm.OptionSectionList = bindingModelOptions.OptionSectionList;
            bm.OptionColorList = bindingModelOptions.OptionColorList;
            bm.ColoredProductsList = bindingModelProducts.ColoredProductsList;

            return View(bm);
        }

        // Во все параметры можно передавать один аргумент в виде класса со свойствами, вместо отдельных аргументов

        [HttpPost]
        public async Task<IActionResult> DuplicateProduct(int productId, int colorId, bool showAll, string section, string category)
        {
            ProductsManagementBindingModel bindingModel = new ProductsManagementBindingModel();

            string message = await _iODatabase.DuplicateProductt(productId, colorId);

            var bindingModelOptions = await _iODatabase.GetOptionNames();

            var bindingModelProducts = await _iODatabase.GetFilteredCatalogProducts(showAll, section, category);

            bindingModel.OptionCategoryList = bindingModelOptions.OptionCategoryList;
            bindingModel.OptionSectionList = bindingModelOptions.OptionSectionList;
            bindingModel.ColoredProductsList = bindingModelProducts.ColoredProductsList;
            bindingModel.SelectedShowAll = true;
            bindingModel.SelectedSection = section;
            bindingModel.SelectedCategory = category;
            bindingModel.Message = message;

            return View("ProductsManagement", bindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId, int colorId, bool showAll, string section, string category)
        {
            ProductsManagementBindingModel bindingModel = new ProductsManagementBindingModel();

            string message = await _iODatabase.DeleteProductt(productId, colorId);

            var bindingModelOptions = await _iODatabase.GetOptionNames();

            var bindingModelProducts = await _iODatabase.GetFilteredCatalogProducts(showAll, section, category);

            bindingModel.OptionCategoryList = bindingModelOptions.OptionCategoryList;
            bindingModel.OptionSectionList = bindingModelOptions.OptionSectionList;
            bindingModel.ColoredProductsList = bindingModelProducts.ColoredProductsList;
            bindingModel.SelectedShowAll = true;
            bindingModel.SelectedSection = section;
            bindingModel.SelectedCategory = category;
            bindingModel.Message = message;

            return View("ProductsManagement", bindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductForm(CreateProductBindingModel bm)
        {

            if ( 
                (ModelState["SelectedCategoryCreateProductForm"].Errors.Count == 0) && 
                (ModelState["SelectedSectionCreateProductForm"].Errors.Count == 0) && 
                (ModelState["SelectedColorCreateProductForm"].Errors.Count == 0) ) 
            {
                var bm1 = await _iODatabase.GetCategoryCharacteristicsWithAllValues(bm);

                bm.ColoredProduct = bm1.ColoredProduct;
                bm.CategoryCharacteristicsList = bm1.CategoryCharacteristicsList;
                bm.OptionCharacteristicValuesList = bm1.OptionCharacteristicValuesList;
                bm.InputCharacteristicValues = bm1.InputCharacteristicValues;
                bm.AllCharacteristicValues = bm1.AllCharacteristicValues;
                bm.NewCategoryId = bm1.NewCategoryId;
                bm.NewColorId = bm1.NewColorId;
                bm.NewSectionId = bm1.NewSectionId;
                bm.ShowProduct = bm1.ShowProduct;

                return View(bm);
            }
            else
            {
                ProductsManagementBindingModel pMBm = new ProductsManagementBindingModel();

                var bindingModelOptions = await _iODatabase.GetOptionNames();

                var bindingModelProducts = await _iODatabase.GetFilteredCatalogProducts(true, "All", "All");

                pMBm.OptionCategoryList = bindingModelOptions.OptionCategoryList;
                pMBm.OptionSectionList = bindingModelOptions.OptionSectionList;
                pMBm.OptionColorList = bindingModelOptions.OptionColorList;
                pMBm.ColoredProductsList = bindingModelProducts.ColoredProductsList;
                pMBm.SelectedShowAll = true;
                pMBm.SelectedSection = "All";
                pMBm.SelectedCategory = "All";
                pMBm.Message = "Для создания товара выберите корректные значения из списков";
                return View("ProductsManagement", pMBm);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductBindingModel bm, int[] categoryCharacteristicsIds, string[] valueNames,  string[] toDeleteValueNames, string[] inputValues)
        {
            bool modelState = false;

            if (ModelState["Files"].Errors.Count == 0)
            modelState = true;


            var productWithMessage = await _iODatabase.CreateProduct(bm, modelState, categoryCharacteristicsIds, valueNames);

            EditProductBindingModel bindingModel = new EditProductBindingModel();
            bindingModel = await _iODatabase.GetColoredProductWithValues(productWithMessage.ProductId, productWithMessage.ColorId);

            bindingModel.InputMessage = "Новый товар создан и доступен в Менеджере товаров";

            return View("EditProduct", bindingModel);
        }

        public async Task<IActionResult> OrdersManagement()
        {
            OrderManagementBindingModel bm = new OrderManagementBindingModel();

            bm.ShowAllOrders = true;

            var bmNew = await _iODatabase.LoadOrders(bm);

            return View(bmNew);
        }

        [HttpPost]
        public async Task<IActionResult> OrdersManagement(OrderManagementBindingModel bm)
        {
            var purchases = await _iODatabase.LoadOrders(bm);

            return View(purchases);
        }

        public async Task<IActionResult> GetClient(OrderManagementBindingModel bm)
	    {
                var client = await _iODatabase.GetClientInfo(bm);
            
	            return Json(client);
	    }

        public async Task<IActionResult> SaveAdminComment(OrderManagementBindingModel bm)
        {
            var adminComment = await _iODatabase.SaveAndReturnAdminComment(bm);

            // чтобы сообщение приходило в браузер не само по себе, а в виде {"adminComment":"message"}
            var adminCommentJson = new{ AdminComment = adminComment }; 

            return Json(adminCommentJson);
        }

        public async Task<IActionResult> SaveCompleted(OrderManagementBindingModel bm)
        {
            var completed = await _iODatabase.SaveAndReturnCompleted(bm);

            // чтобы сообщение приходило в браузер не само по себе, а в виде {"completed":"message"}
            var completedJson = new { Completed = completed };

            return Json(completedJson);
        }
        
        public async Task<IActionResult> CategoryEditor()
        {
            string message = "The section is under development";
            //var purchases = await _iODatabase.LoadOrders(bm);

            return View("CategoryEditor", message);
        }

        public async Task<IActionResult> LogView([FromServices] LogReaderService _logReaderService)
        {
            string message = await _logReaderService.ReadLog();

            return View("LogView", message);
        }

    }
}
