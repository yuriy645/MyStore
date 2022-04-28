using Microsoft.AspNetCore.Http;
using MyStore.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class CartService
    {
        //Отправка в сессию выбранного товара
        public string GetProductToCart(HttpContext httpContext, ColoredProduct coloredProduct)
        {
            var employee = httpContext.Session.Get<Employee>("authorizedEmployee");
            if (employee != null)
            {
                return "Для совершения покупок выйдите из акаунта Сотрудника";
            }
            else
            {
                //Нужно работать с клиентом
                //Берем его из сессии
                var client = httpContext.Session.Get<Client>("authorizedClient");

                //Или наскоро создаём
                if (client == null)
                {
                    client = new Client()
                    {
                        //Заполнение полей, которые нужны для БД, но в экспресс варианте можно не вводить
                        ClientId = 0,
                        RegisterDate = DateTime.Now,
                        Email = "New_client@_" + Guid.NewGuid(),
                        PassHash = PasswordHasher.HashPassword("x"),
                        Name = "Покупатель"
                    };
                }

                //Если заказ еще не прицеплен к клиенту, то создаём его
                if (client.Order == null)
                {
                    client.Order = new Order();
                    client.Order.OrderId = 0;
                    client.Order.OrderDate = DateTime.Now;
                    client.Order.Completed = false;

                    client.Order.DeliveryType = new DeliveryType();
                    client.Order.DeliveryType.DeliveryTypeName = "-"; //имитация заполнения в форме
                    
                    client.Order.Summ = 0;

                    if (client.Order.Purchases == null)
                    {
                        client.Order.Purchases = new List<Purchase>();
                    }
                }

                //Формируем новую покупку
                var purchaseNew = new Purchase() // новая пришедшая покупка
                {
                    OrderId = 0,
                    ProductId = coloredProduct.ProductId,
                    ColorId = coloredProduct.ColorId,
                    Quantity = coloredProduct.CartQuantity,
                    ColoredProduct = coloredProduct
                };
                
               

                //Проверка, есть ли уже такой товар в покупках
                var existsPurchase = client.Order.Purchases // список покупок, кот уже есть
                    .Where(p => p.ColorId == purchaseNew.ColorId)
                    .Where(p => p.ProductId == purchaseNew.ProductId)
                    .FirstOrDefault();

                if (existsPurchase != null) 
                {
                    //Если такой товар уже есть в корзине, то 
                    //увеличиваем на 1 количество этого товара в покупках
                    if (coloredProduct.Price != null)
                    {
                        
                        foreach (var purchase in client.Order.Purchases)
                        {
                            if ( (purchase.ProductId == existsPurchase.ProductId) && (purchase.ColorId == existsPurchase.ColorId) )
                            {
                                purchase.Quantity = purchase.Quantity + purchaseNew.Quantity;
                            }
                        }
                    }
                }
                else
                {
                    //Присоединяем к существующему заказу выбранный товар
                    client.Order.Purchases.Add(purchaseNew);
                }

                

                //Убираем первичный ключ из coloredProduct, чтоб потом не добавлялся при сохранении в таблицу
                coloredProduct.ProductId = 0;
                coloredProduct.ColorId = 0;

                //Пересчет суммы заказа
                if (coloredProduct.Price != null)
                {
                    client.Order.Summ = client.Order.Summ + (decimal)coloredProduct.Price * purchaseNew.Quantity;
                }

                //Сохранение клиента в сессию
                httpContext.Session.Set<Client>("authorizedClient", client);

                return $" \n{coloredProduct.ProductName} - {coloredProduct.CartQuantity} шт. \n\nТовар добавлен в корзину.";
            }
        }
    }
}
