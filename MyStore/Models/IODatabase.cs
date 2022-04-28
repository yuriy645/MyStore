using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyStore;
using MyStore.Models.StoreEntities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using MyStore.Models.DBEntities;
using MyStore.Models.Admin;
using MyStore.Models.Admin.BindingModels;

namespace MyStore.Models
{
    public class IODatabase
    {
        public async Task<Client> GetClientFromDB(Client inClient)
        {
            MyStoreContext db = new MyStoreContext();

            Client client = await db.Clients
                       .Where(b => b.Name == inClient.Name)
                       .Where(b => b.PassHash == inClient.PassHash)
                       .FirstOrDefaultAsync();
            return client;

        }

        //Удаление картинки товара
        public async Task< List<string> > DelImage(int productId, int colorId, int imageId)
        {
            List<string> messages = new List<string>();

            using (MyStoreContext db = new MyStoreContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Удаление из табл. Images
                        var image = /*await*/ db.Images
                              .Where(p => p.ImageId == imageId)
                              .FirstOrDefault();

                       // if ((image != null)&&(image.ImageId != null))
                        {
                            db.Images.Remove(image);
                            /*await*/ db.SaveChanges();
                            messages.Add("Изображение удалено из табл. с картинками");
                        }
                        //else 
                        //{
                        //    messages.Add("Изображение не найдено");
                        //}
                   

                        // Проверка, остались ли еще изображения у товара
                        var imag = await db.Images
                                .Select(p => new { p.ProductId, p.ColorId })
                                .Where(p => p.ProductId == productId)
                                .Where(p => p.ColorId == colorId)
                                .FirstOrDefaultAsync();

                        // Если не остались, то и аватарку убираем
                        if (imag == null)
                        {
                            var coloredProduct = await db.ColoredProducts
                                    .Where(p => p.ProductId == productId)
                                    .Where(p => p.ColorId == colorId)
                                    .FirstOrDefaultAsync();

                            coloredProduct.ImgResized = null; 

                            db.ColoredProducts.Attach(coloredProduct);
                            await db.SaveChangesAsync();
                            messages.Add("Миниатюра удалена из табл. с товарами");
                        }
                        else
                        {
                            messages.Add("Миниатюра НЕ удалена из табл. с товарами");
                        }


                        
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        messages.Add($"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ");
                    }
                }
            }

            return messages;
        }

        //Сохранение и возврат статуса завершения заказа
        public async Task<bool> SaveAndReturnCompleted(OrderManagementBindingModel bm)
        {
            bool completed = bm.Completed;
            int orderId = bm.OrderId;

            using (MyStoreContext db = new MyStoreContext())
            {
                var order = await db.Orders
                   .Where(p => p.OrderId == orderId)
                   .FirstOrDefaultAsync();

                order.Completed = completed;

                db.Orders.Attach(order);

                await db.SaveChangesAsync();

                var orderNew = await db.Orders
                   .Select(p => new { p.OrderId, p.Completed })
                   .Where(p => p.OrderId == orderId)
                   .FirstOrDefaultAsync();

                return orderNew.Completed;
            }
        }


        //Сохранение и возврат комментария Администратора
        public async Task<string> SaveAndReturnAdminComment(OrderManagementBindingModel bm)
        {
            string adminComment = bm.AdminComment;
            int orderId = bm.OrderId;

            using (MyStoreContext db = new MyStoreContext())
            {
                var order = await db.Orders
                   .Where(p => p.OrderId == orderId)
                   .FirstOrDefaultAsync();
               
                order.AdminComment = adminComment;

                db.Orders.Attach(order);

                await db.SaveChangesAsync();

                var orderNew = await db.Orders
                   .Select(p => new {p.OrderId, p.AdminComment})
                   .Where(p => p.OrderId == orderId)
                   .FirstOrDefaultAsync();

                return orderNew.AdminComment;
            }
        }

        //Загрузка всех данных клиента
        public async Task<Client> GetClientInfo(OrderManagementBindingModel bm)
        {
            Client client = new Client();

            using (MyStoreContext db = new MyStoreContext())
            {
                client = await db.Clients
                   .Where(p => p.Email == bm.Client.Email)
                   .FirstOrDefaultAsync();
            }

            OrderManagementBindingModel bmNew = new OrderManagementBindingModel();
            bmNew.Client = client;
            return client;
        }

        //Загрузка таблицы заказов
        public async Task<OrderManagementBindingModel> LoadOrders(OrderManagementBindingModel bm)
        {
            List<ManagementOrder> managementOrders = new List<ManagementOrder>();

            //foreach (var managementOrder in managementOrders)
            //{
            //    List<ManagementPurchase> ManagementPurchases = new();
            //}

            
            using (MyStoreContext db = new MyStoreContext())
            {
                
                var purchases = await db.Purchases
                    .Include(p => p.Order)
                    .Include(p => p.Order.Client)
                    .Include(p => p.Order.DeliveryType)
                    .Include(p => p.Order.Employee)
                    .Include(p => p.ColoredProduct)
                    //.Select(p => new 
                    //{
                    //    OrderId = p.OrderId,
                    //    OrderDate = p.Order.OrderDate,
                    //    ClientEmail = p.Order.Client.Email,
                    //    Comment = p.Order.Comment,
                    //    AdminComment = p.Order.AdminComment,
                    //    EmployeeName = p.Order.Employee.Name,
                    //    EmployeeSecondName = p.Order.Employee.SecondName,
                    //    Completed = p.Order.Completed,
                    //    DeliveryTypeName = p.Order.DeliveryType.DeliveryTypeName,

                    //    ProductCode = p.ColoredProduct.ProductCode,
                    //    ImgResized = p.ColoredProduct.ImgResized,
                    //    ProductName = p.ColoredProduct.ProductName,
                    //    Quantity = p.Quantity
                        
                    //})
                    .Where(p => bm.ShowAllOrders == true ? true : p.Order.Completed == false)
                    .OrderByDescending(p => p.OrderId)
                    .ThenByDescending(p => p.Order.OrderDate)
                    .ToListAsync();


                var groups = from p in purchases
                             group p by p.Order;

                //foreach (var group in groups)
                //{
                //    Console.WriteLine(group.Key);

                //    foreach (var p in group)
                //        Console.WriteLine("{0} - {1}", p.ColoredProduct.ProductCode, p.ColoredProduct.ImgResized);

                //    Console.WriteLine();
                //}


                //var groups = purchases
                //    .GroupBy(p => p.Order)
                //    .Select(group => new { order = group.Key, count = group.Count() });

                ManagementOrder managementOrder = null;

                foreach (var group in groups) //проход по группам
                {
                    //создаем заказ с параметрами группы
                    managementOrder = (new ManagementOrder()
                    {
                        OrderId = group.Key.OrderId,
                        OrderDate = group.Key.OrderDate,
                        ClientEmail = group.Key.Client.Email,
                        Comment = group.Key.Comment,
                        AdminComment = group.Key.AdminComment,
                        EmployeeName = group.Key.Employee.Name,
                        EmployeeSecondName = group.Key.Employee.SecondName,
                        Completed = group.Key.Completed,
                        DeliveryTypeName = group.Key.DeliveryType.DeliveryTypeName,
                        ManagementPurchases = new List<ManagementPurchase>()  //инициализация коллекции покупок в заказе
                    });

                    //добавляем список покупок в заказ
                    foreach (var p in group) //проход по содержимому групп
                    {
                        managementOrder.ManagementPurchases.Add(new ManagementPurchase()
                        {
                            ProductCode = p.ColoredProduct.ProductCode,
                            ImgResized = p.ColoredProduct.ImgResized,
                            ProductName = p.ColoredProduct.ProductName,
                            Quantity = p.Quantity
                        });
                    }

                    //добавляем заказ со списком покупок в список заказов
                    managementOrders.Add(managementOrder);
                }
            }

            OrderManagementBindingModel bmNew = new OrderManagementBindingModel();
            bmNew.ManagementOrders = managementOrders;
            bmNew.ShowAllOrders = bm.ShowAllOrders;
            return bmNew;   
        }


        //Удалить товар из БД
        public async Task<string> DeleteProductt(int productId, int colorId)
        {
            
            string message = null;

            using (MyStoreContext db = new MyStoreContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Скачиваем картинки
                        var images = db.Images
                            .Where(p => p.ProductId == productId)
                            .Where(p => p.ColorId == colorId)
                            .ToList();

                        if (images.Count != 0 )
                        {
                            db.Images.RemoveRange(images);
                            db.SaveChanges();
                        }


                        var purchases = db.Purchases
                            .Select(p => new { p.ProductId, p.ColorId })
                            .Where(p => p.ProductId == productId)
                            .Where(p => p.ColorId == colorId)
                            .ToList();

                        if (purchases.Count != 0)
                        {
                            return "Нельзя удалить, товар пока он содержится в заказах";
                            //db.Orders.RemoveRange(orders);
                            //db.SaveChanges();
                        }

                        
                        
                        var coloredProduct = db.ColoredProducts
                            .Where(p => p.ProductId == productId)
                            .Where(p => p.ColorId == colorId)
                            .FirstOrDefault();
                        
                        db.ColoredProducts.Remove(coloredProduct);
                        db.SaveChanges();


                        var productValues = db.ProductValues
                           .Where(p => p.ProductId == productId)
                           .ToList();

                        db.ProductValues.RemoveRange(productValues);
                        db.SaveChanges();


                        var product = db.Products
                            .Where(p => p.ProductId == productId)
                            .FirstOrDefault();

                        db.Products.Remove(product);
                        await db.SaveChangesAsync();

                        transaction.Commit();

                        message = "Удаление товара завершено";

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return message = ex.Message;
                    }
                }
            }
            return message;
        }

        //Создать дубликат товара в БД
        public async Task<string> DuplicateProductt(int productId, int colorId)
        {
            int newProductId = 0;
            //int newColorId = 0;
            string message = null;
            string prodCode = null;
            using (MyStoreContext db = new MyStoreContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Скачиваем 1 товар из табл. Products
                        var product = db.Products
                            .Where(p => p.ProductId == productId)
                            .FirstOrDefault();
                
                        //Сбрасываем его Id
                        product.ProductId = 0;
                        
                        //Сохраняем новый товар в табл Products
                        db.Products.Add(product);
                        db.SaveChanges();
                        //*****************
                
                
                        //Определяем Id нового товара
                        newProductId = db.Products
                           // .Where(p => p.Description == product.Description) //это проверка на всякий случай
                            .Max(p => p.ProductId);
                
                        //Скачиваем список соответствий: характеристика в категории - значение для товара
                        //сами соответствия уже есть т. к. это дубликат
                        var productValues = db.ProductValues
                            .Where(p => p.ProductId == productId)
                            .ToList();
                
                        //В этом списке каждому соответствию подменяем ProductId на новый
                        //и сбрасываем его Id
                        foreach (var productValue in productValues)
                        {
                            productValue.ProductId = newProductId;
                            productValue.ProductValuesId = 0;
                        }
                
                        //Сохраняем список нового товара в табл. ProductValues
                        db.ProductValues.AddRange(productValues);
                       // db.SaveChanges();//
                
                        //************************
                
                        //Скачиваем 1 товар из табл. ColoredProducts
                        var coloredProduct = db.ColoredProducts
                            .Where(p => p.ProductId == productId)
                            .Where(p => p.ColorId == colorId)
                            .FirstOrDefault();
                
                        //Собираем новый составной ключ и меняем уникальный код товара
                        coloredProduct.ProductId = newProductId;
                        coloredProduct.ColorId = colorId;
                        prodCode = coloredProduct.ProductCode;
                        coloredProduct.ProductCode = coloredProduct.ProductCode + " ДУБЛИКАТ     " + Guid.NewGuid().ToString();
                
                        //Сохраняем новый товар в табл. ColoredProducts
                        db.ColoredProducts.Add(coloredProduct);
                        //db.SaveChanges();
                
                        //Скачиваем картинки
                        var images = db.Images
                            .Where(p => p.ProductId == productId)
                            .Where(p => p.ColorId == colorId)
                            .ToList();
                
                        foreach (var image in images)
                        {
                            image.ImageId = 0;
                            image.ProductId = newProductId;
                            image.ColorId = colorId;
                        }
                
                        db.Images.AddRange(images);

                        await db.SaveChangesAsync();

                        transaction.Commit();
                        message = "Создан дубликат товара " + prodCode; 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        message = ex.InnerException.ToString();
                        
                    }
                }
            }
            return message;
        }

        //В текущей версии не используется
        //Сохранить цвет
        public async Task SaveColor(int productId, int colorId, string colorName)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим новое значение ColorId
                var newColor = await db.Colors
                    .Where(p => p.ColorName == colorName)
                    .FirstOrDefaultAsync();
                // получили newColor.ColorId

                //Проверяем, существует ли уже этот товар с желаемым ColorId
                var existsColoredProduct = await db.ColoredProducts
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == newColor.ColorId)
                    .FirstOrDefaultAsync();
                if (existsColoredProduct == null) //если нет такого товара, то спокойно изменяем цвет
                {
                    //находим цветной продукт, который необходимо изменить и изменяем
                    var newColoredProduct = await db.ColoredProducts
                        .Where(p => p.ProductId == productId)
                        .Where(p => p.ColorId == colorId)
                        .FirstOrDefaultAsync();


                    //принимаем на отслеживание - ЭТО НЕ РАБОТАЕТ  С ЧАСТЬЮ ГЛАВНОГО КЛЮЧА
                     db.ColoredProducts.Attach(newColoredProduct);

                    //var images = db.Images;
                    //   // .Where(p => p.ProductId == productId)
                    //   // .Where(p => p.ColorId == colorId);
                    //var tempImages = images;
                    //db.Images.RemoveRange(images);
                    //db.SaveChanges();

                    //var orders = db.Orders;
                    //   // .Where(p => p.ProductId == productId)
                    //   // .Where(p => p.ColorId == colorId);
                    //var tempOrders = orders;
                    //db.Orders.RemoveRange(orders);
                    //db.SaveChanges();


                    //newColoredProduct.ColorId = newColor.ColorId;
                    //db.SaveChanges();
                    ////теперь у этого товара нет картинок и заказов
                }
                else 
                { 
                //сообщение  какое-то
                //"Данный товар с таким цветом уже существует, цвет не изменен"
                }   
            }
        }


        //Сохранить описание
        public async Task SaveDescription(int productId, string description)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим цветной продукт, который необходимо изменить и изменяем
                var product = await db.Products
                    .Where(p => p.ProductId == productId)
                    .FirstOrDefaultAsync();

                product.Description = description;

                //принимаем на отслеживание
                db.Products.Attach(product);

                await db.SaveChangesAsync();
            }
        }

        //Сохранить настройку, нужно ли показывать на сайте
        public async Task SaveShowProduct(int productId, int colorId, bool showProduct)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим цветной продукт, который необходимо изменить и изменяем
                var coloredProduct = await db.ColoredProducts
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                coloredProduct.ShowProduct = showProduct;

                //принимаем на отслеживание
                db.ColoredProducts.Attach(coloredProduct);

                await db.SaveChangesAsync();
            }
        }

        //Сохранить остаток на складе
        public async Task SaveStock(int productId, int colorId, int stock)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим цветной продукт, который необходимо изменить и изменяем
                var coloredProduct = await db.ColoredProducts
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                coloredProduct.Stock = stock;

                //принимаем на отслеживание
                db.ColoredProducts.Attach(coloredProduct);

                await db.SaveChangesAsync();
            }
        }

        //Сохранить цену
        public async Task SavePrice(int productId, int colorId, decimal price)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим цветной продукт, который необходимо изменить и изменяем
                var coloredProduct = await db.ColoredProducts
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                coloredProduct.Price = price;

                //принимаем на отслеживание
                db.ColoredProducts.Attach(coloredProduct);

                await db.SaveChangesAsync();
            }
        }

        //Сохранить код товара
        public async Task SaveProductCode(int productId, int colorId, string productCode)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим цветной продукт, который необходимо изменить и изменяем
                var coloredProduct = await db.ColoredProducts
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                coloredProduct.ProductCode = productCode;

                //принимаем на отслеживание
                db.ColoredProducts.Attach(coloredProduct);

                await db.SaveChangesAsync();
            }
        }

        //Сохранить название товара
        public async Task SaveProductName(int productId, int colorId, string productName)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                //находим цветной продукт, который необходимо изменить и изменяем
                var coloredProduct = await db.ColoredProducts
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                coloredProduct.ProductName = productName;

                //принимаем на отслеживание
                db.ColoredProducts.Attach(coloredProduct);

               await db.SaveChangesAsync();
            }
        }


        //Добавление значения из списка характеристик категории к товару 
        public List<string> AddValue(string[] ValueNames, int[] addCategoryCharacteristicsIds, int productId)
        {
            List<string> messages = new List<string>();

            using (MyStoreContext db = new MyStoreContext())
            {
                for (int i = 0; i < ValueNames.Length; i++)
                {
                   

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                       //Ищем сочетание ИмяЗначения + Id характеристики в категории табл. 7, оно там есть, т к ИмяЗначения приходит из списка.
                       //И оно возможно в табл 7 только одно. 
                       //Если присоединить табл 15 с уточнением по ProductId, то можно проверять выборку на null.
                       //Искать тоже одно значение
                       var productValue = db.ProductValues
                           .Include(p => p.CharacteristicValue)
                           .Include(p => p.CharacteristicValue.Value)
                           .Where(p => p.CharacteristicValue.Value.ValueName == ValueNames[i])
                           .Where(p => p.CharacteristicValue.CategoryCharacteristicsId == addCategoryCharacteristicsIds[i])
                           .Where(p => p.ProductId == productId)
                           .FirstOrDefault();
                       
                           //Если в табл 15 нет нужной записи, то надо сделать её
                           if (productValue == null)
                           {
                               // удалить старую запись с дугим  Значением и с тем же сочетанием CategoryCharacteristicsId + ProductId
                               // если не удалить, то в табл 15 "наплодятся" лишние записи с одинаковым содержимым и разными Id-шниками.
                               var oldProductValue = db.ProductValues
                               .Include(p => p.CharacteristicValue)
                               .Where(p => p.ProductId == productId)
                               .Where(p => p.CharacteristicValue.CategoryCharacteristicsId == addCategoryCharacteristicsIds[i])
                               .FirstOrDefault();
                           
                               db.ProductValues.Remove(oldProductValue);
                               db.SaveChanges();
                           
                               //1 найти для записи значение в характеристике
                               var characteristicValues = db.CharacteristicValues
                                   .Include(p => p.Value)
                                   .Where(p => p.Value.ValueName == ValueNames[i])
                                   .Where(p => p.CategoryCharacteristicsId == addCategoryCharacteristicsIds[i])
                                   .FirstOrDefault();
                               //2 из найденного значения в характеристики и Id товара создать новый экземпляр для табл 15 
                               ProductValue newProductValue = new ProductValue() {CharacteristicValuesId = characteristicValues.CharacteristicValuesId, ProductId = productId };
                       
                               //добавление нового значения для товара 
                               db.ProductValues.Add(newProductValue);
                               db.SaveChanges();
                                messages.Add($"Измененные значения сохранены.");
                           }
                       
                           
                           transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        messages.Add($"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ");
                    }
                    }
                    

                }
            }
            return messages;
        }

        // Удаление значения из характеристики категории
        // Задача метода - удалить значение из конкретной характеристики категории.Если это возможно
        public List<string> DeleteCharateristicValue(string[] toDeleteValueNames, int[] categoryCharacteristicsIds) //массив наборов valueId + categoryCharacteristicsId (
        {
            List<string> messages = new List<string>();
            string valueName = null;
            int categoryCharacteristicsId = 0;

            using (MyStoreContext db = new MyStoreContext())
            {
                for (int i = 0; i < toDeleteValueNames.Length; i++) 
                {
                    valueName = toDeleteValueNames[i];
                    categoryCharacteristicsId = categoryCharacteristicsIds[i];

                    if (valueName != null)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                //Если сочетанию ValueId + CategoryCharacteristicsId соответствует хоть 1 productId, то  ищем имена всех productId и пишем :
                                //Удалить невозможно. значение характеристики категории _кот пытаемся удалить_ назначено в товарах.  

                                //Иначе
                                //1. удаляем из 7 строку с сочетанием ValueId + CategoryCharacteristicsId
                                //2. проверяем, содержится ли ValueId в 7 хоть 1 раз, если нет- то удаляем строку с ValueId из табл 5
                                //3. пишем значение характеристики категории _кот пытаемся удалить_  удалено
                                var productValues = db.ProductValues// нашли все ProductId, в которые входит значение
                                  .Include(p => p.CharacteristicValue)
                                  .Include(p => p.CharacteristicValue.Value)
                                  .Where(p => p.CharacteristicValue.CategoryCharacteristicsId == categoryCharacteristicsId)
                                  .Where(p => p.CharacteristicValue.Value.ValueName == valueName)
                                  .ToList();

                                if (productValues.Count != 0)
                                {
                                    //Найти имя значения по его Id
                                    //var value = db.Values
                                    //    .Where(p => p.ValueId == valueId)
                                    //    .FirstOrDefault();

                                    //Найти названия товаров для списка ProductId
                                    messages.Add($"Удаление запрещено, т. к. '{valueName}' содержится в товарах: ");
                                    foreach (var productValue in productValues)
                                    {
                                        var coloredProducts = db.ColoredProducts//берем товары всех цветов и вытягиваем с теми ProductId, которые нашлись по значениям, кот им принадлежат
                                                                    .Include(p => p.Product)
                                                                    .Where(p => p.ProductId == productValue.ProductId) // == любому из ProductId в списке productValues
                                                                    .ToList();
                                        foreach (var coloredProduct in coloredProducts)
                                        {
                                            messages.Add($"{coloredProduct.ProductName} ; ");
                                        }
                                    }
                                }
                                else
                                {//просто поиск и удаление этой характеристики в категории
                                    var characteristicValue = db.CharacteristicValues
                                        .Include(p => p.Value)
                                        .Where(p => p.CategoryCharacteristicsId == categoryCharacteristicsId)
                                        .Where(p => p.Value.ValueName == valueName)
                                        .FirstOrDefault();
                                    db.CharacteristicValues.Remove(characteristicValue);
                                    db.SaveChanges();
                                    messages.Add($"{valueName} удалено из списка характеристик в категории ");

                                    var characteristicValue1 = db.CharacteristicValues //чтобы узнать, принадлежитли значение другим характеристикам и/или категориям
                                        .Include(p => p.Value)
                                        .Where(p => p.Value.ValueName == valueName)
                                        .FirstOrDefault();
                                    if (characteristicValue1 == null) // если не принадлежит, то оно не используется в табл 5 и может быть совсем удалено из БД
                                    {
                                        var value = db.Values
                                            .Where(p => p.ValueName == valueName)
                                            .FirstOrDefault();
                                        db.Values.Remove(value);
                                        db.SaveChanges();
                                        messages.Add($"{valueName} удалено из БД ");
                                    }

                                    
                                   
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                messages.Add($"Exception Message: {ex.Message}, {ex.Source}, {ex.Data} ");
                            }
                        }
                    }
                }
            }
            return messages;
        }

        //Добавление значения в характеристику категории 
        public List<string> AddCharacteristicValue(string[] inputValues, int[] addCategoryCharacteristicsIds)
        {
             List<string> messages = new List<string>();
            
             using (MyStoreContext db = new MyStoreContext())
             {
                 for (int i = 0; i < inputValues.Length; i++)
                 {
                     if (inputValues[i] != null)
                     {

                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var values = db.Values.ToList();
                                //.Include(p => p.CharacteristicValues);

                                int toAddvalueId = 0;

                                //проверяем, есть ли значение уже в табл Values
                                var oldValue = values
                                    .Where(p => p.ValueName == inputValues[i])
                                    .FirstOrDefault();

                                if (oldValue != null) //если уже есть это значение в Value - то запомнить его Id
                                {
                                    toAddvalueId = oldValue.ValueId;
                                    messages.Add($"{inputValues[i]} уже было записано в табл. Values; ");
                                }
                                else //если в табл Value еще нет этого значения, то записать в Value и определить новое Id Value 
                                {
                                    //toAddvalueId = values.Max(v => v.ValueId) + 1; //Выясняем Id, с которым будет записано новое значение
                                    //почему-то после внесения и удаления строки в Vaues, нумерация Id начиналась со следующего Id, как будто удаления не было.

                                    //db.ProductValues.Add(newProductValue);
                                    var newValue = new Value() { ValueName = inputValues[i] };
                                    db.Values.Add(newValue); //Запись значения в табл. Values
                                    db.SaveChanges();

                                    var value = db.Values
                                        .Where(p => p.ValueName == inputValues[i])
                                        .FirstOrDefault();

                                    toAddvalueId = value.ValueId;
                                }

                                //проверка, содержит ли табл 7 CharacteristicValues запись с такими данными, как нужно записать
                                var oldCharacteristicValues = db.CharacteristicValues
                                    .Where(p => p.ValueId == toAddvalueId)
                                    .Where(p => p.CategoryCharacteristicsId == addCategoryCharacteristicsIds[i])
                                    .FirstOrDefault();

                                if (oldCharacteristicValues != null) //если есть - сообщаем об этом и выходим
                                {
                                    messages.Add($"{inputValues[i]} уже есть в характеристике категории; ");
                                }
                                else // если не было - создаем эту запись и сохраняем в БД
                                {
                                    var newCharacteristicValue = new CharacteristicValue() { ValueId = toAddvalueId, CategoryCharacteristicsId = addCategoryCharacteristicsIds[i] };
                                    db.CharacteristicValues.Add(newCharacteristicValue);
                                    db.SaveChanges();

                                    messages.Add($"{inputValues[i]} успешно добавлено в характеристику категории; ");
                                }
                                
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                messages.Add($"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ");
                            }
                        }
                    }
                     
                 }
             }
             return messages;   
        }

        //Загрузка списка цветов 
        public async Task<EditProductBindingModel> GetOptionColors()
        { 
            List<Color> colorList = new List<Color>();
            List<SelectListItem> optionColorList = new List<SelectListItem>();

            using (MyStoreContext db = new MyStoreContext())
            { 
               colorList = await db.Colors.ToListAsync();
            }

            foreach (var color in colorList)
            {
                optionColorList.Add(new SelectListItem(color.ColorName, color.ColorName));
            }

            EditProductBindingModel bindingModel = new EditProductBindingModel();
            bindingModel.OptionColorsList = optionColorList;

            return bindingModel;
        }

        //Загрузка списков категорий, разделов и цветов выпадающих меню 
        public async Task<ProductsManagementBindingModel> GetOptionNames()
        {
            List<Section> sectionList = new List<Section>();
            List<Category> categoryList = new List<Category>();
            List<Color> colorList = new List<Color>();

            using (MyStoreContext db = new MyStoreContext())
            {
                sectionList = await db.Sections.ToListAsync();
                categoryList = await db.Categories.ToListAsync();
                colorList = await db.Colors.ToListAsync();
            }

            List<SelectListItem> optionSectionList = new List<SelectListItem>();
            foreach (var section in sectionList)
            {
                optionSectionList.Add(new SelectListItem(section.SectionName, section.SectionName));
            }
            optionSectionList.Add(new SelectListItem("All", "All"));//чтобы не вносить В БД выбор "All"

            List<SelectListItem> optionCategoryList = new List<SelectListItem>();
            foreach (var category in categoryList)
            {
                optionCategoryList.Add(new SelectListItem(category.CategoryName, category.CategoryName));
            }
            optionCategoryList.Add(new SelectListItem("All", "All"));//чтобы не вносить В БД выбор "All"

            List<SelectListItem> optionColorList = new List<SelectListItem>();
            foreach (var color in colorList)
            {
                optionColorList.Add(new SelectListItem(color.ColorName, color.ColorName));
            }

            ProductsManagementBindingModel bindingModel = new ProductsManagementBindingModel();
            bindingModel.OptionSectionList = optionSectionList;
            bindingModel.OptionCategoryList = optionCategoryList;
            bindingModel.OptionColorList = optionColorList;

            return bindingModel;
        }

        //Загрузка каталога товаров с настраиваемой фильтрацией
        public async Task<ProductsManagementBindingModel> GetFilteredCatalogProducts(bool showAll, string section, string category)
        {
            List<ColoredProduct> coloredProductsList = new List<ColoredProduct>();

            using (MyStoreContext db = new MyStoreContext())
            {
                coloredProductsList = await db.ColoredProducts
                    .Include(p => p.Product)// Product- нав. св-во в классе ColoredProducts
                    .Include(p => p.Product.Category)
                    .Include(p => p.Product.Section)
                    .Include(p => p.Color)
                    .OrderBy(p => p.ProductCode)
                    .ToListAsync();
            }

             var coloredProductsFilteredList = coloredProductsList

                .Where(p => showAll == true ? true : (p.ShowProduct == true))

                .Where(p => section == "All" ? true : (p.Product.Section.SectionName == section))

                .Where(p =>
                {
                    if (category == "All")
                        return true;
                    else
                        return p.Product.Category.CategoryName == category;
                }
                      );

            //var images = await db.Images
            //        .Where(p => p.ProductId == productId)
            //        .Where(p => p.ColorId == colorId)
            //        .ToListAsync();


            ProductsManagementBindingModel bindingModel = new ProductsManagementBindingModel();
            bindingModel.ColoredProductsList = coloredProductsFilteredList.ToList();

            return bindingModel;
        }

        //Загрузка каталога товаров для покупателей
        public async Task<List<ColoredProduct>> GetCatalogProducts(string section, string category)
        {
            using (MyStoreContext db = new MyStoreContext())
            {
                var coloredProducts = await db.ColoredProducts // Можно было бы не использовать табл. Categories и Sections,
                                                               // если передавать из представления Id категории и раздела, а не их имена.
                    .Include(p => p.Product)// Product- нав. св-во в классе ColoredProducts
                      .ThenInclude(p => p.Category)
                    .Include(p => p.Product.Section)
                    .Include(p => p.Color)

                    .Where(p => p.Product.Section.SectionName == section) //SectionId принадлежит табл.Products, обратились к нему через нав. св-во Product класса ColoredProducts
                    .Where(p => p.Product.Category.CategoryName == category)
                    .Where(p => p.ShowProduct == true)
                    .ToListAsync();
                return coloredProducts;
            }
        }
        
        
        
        //Загрузка подробностей одного товара 
        public async Task<ColoredProduct> GetColoredProduct(int productId, int colorId)
        {

            using (MyStoreContext db = new MyStoreContext())
            {
                //Грузим общие данные по одному товару
                var coloredProduct = await db.ColoredProducts
                   // .Include(p => p.ColoredProduct)
                    .Include(p => p.Color)
                    .Include(p => p.Product)
                      .ThenInclude(p => p.Category)
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                // Подгрузили список характеристик категории для этого товара.
                var categoryCharacteristics = await db.CategoryCharacteristics
                    .Include(h => h.Characteristic)
                    .Where(p => p.CategoryId == coloredProduct.Product.CategoryId)
                    .OrderBy(h => h.OrdinationNumber)
                    .ToListAsync();


                // Нашли только те значения, которые соответствуют запрашиваемому товару
                var productValues = await db.ProductValues // по сути находим одну строчку из табл. dbo.CharacteristicValues, которая соответствует конкретному товару. 
                                                           // Одну- потому что одному товару соответствует только одно значение текущей характеристики в категории
                    .Include(m => m.CharacteristicValue) //CharacteristicValues- это нав. поле типа CharacteristicValue в классе ProductValue
                    .Include(j => j.CharacteristicValue.Value)                                //
                                                                                              //    .Include(h => h.CategoryCharacteristics)
                                                                                              //.ThenInclude(h => h.Characteristic)
                    .Where(p => p.ProductId == productId)
                    .ToListAsync();

                // Грузим все картинки товара
                var images = await db.Images
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .ToListAsync();

                return coloredProduct;
            }
        }

        //В текущей версии данный метод не используется
        //Загрузка списка характеристик категории (по категории товара с productId), 
        //загрузка массива  списков значений ( OptionsCharacteristicValues[i] ) для конкретной характеристики
        public async Task<EditProductBindingModel> GetCategoryCharacteristicsValues(int productId, int colorId)
        {
            Image image;
            List<CharacteristicValue> characteristicValues;

            using (MyStoreContext db = new MyStoreContext())
            {
                 // Тут категория будет соответствовать productId
                 image = await db.Images 
                    .Include(p => p.ColoredProduct)
                    .Include(p => p.ColoredProduct.Color)
                    .Include(p => p.ColoredProduct.Product)
                    .Include(p => p.ColoredProduct.Product.Category)
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                // Подгрузили список значений характеристик, ограниченных только категорией, какая это характеристика- не учтено
                characteristicValues = await db.CharacteristicValues 
                    .Include(h => h.Value)
                    //.Include(h => h.CharacteristicValues)**
                    .Include(h => h.CategoryCharacteristic)//**
                    //.Include(h => h.CharacteristicValues.Category)
                    .Include(h => h.CategoryCharacteristic.Category)//**
                    //.Include(h => h.CharacteristicValues.Characteristic)**
                    .Include(h => h.CategoryCharacteristic.Characteristic)//**
                    //.Where(p => p.CharacteristicValues.Category.CategoryId == image.ColoredProduct.Product.CategoryId)**
                    .Where(p => p.CategoryCharacteristic.Category.CategoryId == image.ColoredProduct.Product.CategoryId)
                    .ToListAsync();

            }

            //Цель создания этого массива - получение массива списков объектов List<SelectListItem>[]
            //для выпадающих меню для задания индивидуальных характеристик товара в админке
            List<CharacteristicValue>[]  characteristicValuesList = new List<CharacteristicValue>[characteristicValues.Count];
            //characteristicValuesList[0] список для 1-й хар-ки товара
            //characteristicValuesList[1] список для 2-й хар-ки товара...

            for (int i = 0; i < characteristicValues.Count; i++) // проход по значениям характеристики
            {
                //список значений для i-ой характеристики        //список значений, возможных в категории
                characteristicValuesList[i] = characteristicValues
                      //.Where(p => p.CharacteristicValues.OrdinationNumber == i + 1)**
                      .Where(p => p.CategoryCharacteristic.OrdinationNumber == i + 1)
                      .ToList();
            }

            //массив списков значений
            List<SelectListItem>[] optionCharacteristicValuesList = new List<SelectListItem>[characteristicValuesList.Length];

            for (int i = 0; i < characteristicValuesList.Length; i++)
            {
                optionCharacteristicValuesList[i] = new List<SelectListItem>();

                foreach (var value in characteristicValuesList[i])
                {
                    optionCharacteristicValuesList[i].Add(new SelectListItem(value.Value.ValueName, value.Value.ValueName));
                }
            }


            EditProductBindingModel bindingModel = new EditProductBindingModel();
            bindingModel.ColoredProduct = image.ColoredProduct;
            bindingModel.OptionCharacteristicValuesList = optionCharacteristicValuesList;

            
            //await db.ProductValues.Remove(p = productId.)

            return bindingModel;
        }



        //Загрузка подробностей одного товара со всеми значениями
        public async Task<EditProductBindingModel> GetColoredProductWithValues(int productId, int colorId)
        {
            ColoredProduct coloredProduct;
            List<Image> images = new List<Image>();
            List<CharacteristicValue> characteristicValues;
            List<Color> colorList = new List<Color>();
            List<SelectListItem> optionColorList = new List<SelectListItem>();
            EditProductBindingModel bindingModel = new EditProductBindingModel();

            using (MyStoreContext db = new MyStoreContext())
            {
                //Грузим общие данные по одному товару
                coloredProduct = await db.ColoredProducts
                    //.Include(p => p.ColoredProduct)
                    .Include(p => p.Color)
                    .Include(p => p.Product)
                      .ThenInclude(p => p.Category)
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .FirstOrDefaultAsync();

                // Подгрузили список характеристик категории для этого товара.
                var categoryCharacteristics = await db.CategoryCharacteristics
                    .Include(h => h.Characteristic)
                    .Where(p => p.CategoryId == coloredProduct.Product.CategoryId)
                    .OrderBy(h => h.OrdinationNumber)
                    .ToListAsync();


                //нашли только те значения, которые соответствуют запрашиваемому товару
                var productValues = await db.ProductValues // по сути находим одну строчку из табл. dbo.CharacteristicValues, которая соответствует конкретному товару. 
                                                           // Одну- потому что одному товару соответствует только одно значение текущей характеристики в категории
                    .Include(m => m.CharacteristicValue)   //CharacteristicValues- это нав. поле типа CharacteristicValue в классе ProductValue
                    .Include(j => j.CharacteristicValue.Value)
                    .OrderBy(h => h.CharacteristicValue.CategoryCharacteristic.OrdinationNumber)
                    .Where(p => p.ProductId == productId)
                    .ToListAsync();

                //нашли только те значения, которые соответствуют запрашиваемому товару
                //var productValues = await db.ProductValues // по сути находим одну строчку из табл. dbo.CharacteristicValues, которая соответствует конкретному товару. 
                //                                           // Одну- потому что одному товару соответствует только одно значение текущей характеристики в категории
                //    .Include(m => m.CharacteristicValue)   //CharacteristicValues- это нав. поле типа CharacteristicValue в классе ProductValue
                //    .Include(j => j.CharacteristicValue.Value)
                //    .Include(j => j.CharacteristicValue.CategoryCharacteristic)
                //    .Include(j => j.CharacteristicValue.CategoryCharacteristic.Characteristic)
                //    .Where(p => p.ProductId == productId)
                //    .Where(p => p.CharacteristicValue.CategoryCharacteristic.CategoryId == coloredProduct.Product.CategoryId)
                //    .OrderBy(h => h.CharacteristicValue.CategoryCharacteristic.OrdinationNumber)
                //    .ToListAsync();

                //Запоминаем значения характеристик в категории, соответствующие конкретному товару
                bindingModel.ColoredProduct = coloredProduct;
                bindingModel.ColorId = coloredProduct.ColorId;
                bindingModel.ProductId = coloredProduct.ProductId;
                bindingModel.ProductCode = coloredProduct.ProductCode;
                bindingModel.ProductName = coloredProduct.ProductName;
                bindingModel.Price = coloredProduct.Price;
                bindingModel.ShowProduct = coloredProduct.ShowProduct;
                bindingModel.Stock = coloredProduct.Stock;

                //Удалили из контекста лишние данные, чтобы перевыбрать их по другому
                //db.RemoveRange();
                //db.ProductValues.RemoveRange(productValues);

                images = await db.Images
                    .Where(p => p.ProductId == productId)
                    .Where(p => p.ColorId == colorId)
                    .ToListAsync();
            }

            using (MyStoreContext db = new MyStoreContext())
            {
                // Подгрузили список значений характеристик, ограниченных только категорией, какая это характеристика- не учтено
                characteristicValues = await db.CharacteristicValues 
                    .Include(h => h.Value)
                    .Include(h => h.CategoryCharacteristic)
                    .Include(h => h.CategoryCharacteristic.Category)
                    .Include(h => h.CategoryCharacteristic.Characteristic)
                    .Where(p => p.CategoryCharacteristic.Category.CategoryId == coloredProduct.Product.CategoryId)
                    .ToListAsync();

                colorList = await db.Colors.ToListAsync();
            }


            //Цель создания этого массива - получение массива списков объектов List<SelectListItem>[]
            //для выпадающих меню для задания индивидуальных характеристик товара в админке
            List<CharacteristicValue>[] characteristicValuesList = new List<CharacteristicValue>[characteristicValues.Count];
            //characteristicValuesList[0] список для 1-й хар-ки товара
            //characteristicValuesList[1] список для 2-й хар-ки товара...

            for (int i = 0; i < characteristicValues.Count; i++) // проход по значениям характеристики
            {
                //список значений для i-ой характеристики        //список значений, возможных в категории
                characteristicValuesList[i] = characteristicValues
                      .Where(p => p.CategoryCharacteristic.OrdinationNumber == i + 1)
                      .ToList();
            }

            //Массив списков значений
            List<SelectListItem>[] optionCharacteristicValuesList = new List<SelectListItem>[characteristicValuesList.Length];

            for (int i = 0; i < characteristicValuesList.Length; i++)
            {
                optionCharacteristicValuesList[i] = new List<SelectListItem>();

                foreach (var value in characteristicValuesList[i])
                {
                    optionCharacteristicValuesList[i].Add(new SelectListItem(value.Value.ValueName, value.Value.ValueName));
                }
            }


            foreach (var color in colorList)
            {
                optionColorList.Add(new SelectListItem(color.ColorName, color.ColorName));
            }


            //Запоминаем выпадающий список значений цветов
            bindingModel.OptionColorsList = optionColorList;

            //Запоминаем выпадающие списки значений характеристик категории
            bindingModel.OptionCharacteristicValuesList = optionCharacteristicValuesList;


            return bindingModel;
        }
        


        //Инициализация пустой формы создания товара ( предзаполнение + загрузка характеристик категории + загрузка списков значений в характеристиках )
        public async Task<CreateProductBindingModel> GetCategoryCharacteristicsWithAllValues(CreateProductBindingModel bm)
        {
            string sectionName = bm.SelectedSectionCreateProductForm;
            string categoryName = bm.SelectedCategoryCreateProductForm;
            string colorName = bm.SelectedColorCreateProductForm;

            int newColorId = 0;
            int newCategoryId = 0;
            int newSectionId = 0;

            ColoredProduct newColoredProduct = new ColoredProduct();
            newColoredProduct.Product = new Product();

            List<CategoryCharacteristic> categoryCharacteristics = null;
            List<CharacteristicValue> allCharacteristicValues;
            
            CreateProductBindingModel bindingModel = new CreateProductBindingModel();

            using (MyStoreContext db = new MyStoreContext())
            {
                //Определяем ColorId
                var color = db.Colors
                    .Where(p => p.ColorName == colorName)
                    .FirstOrDefault();
                newColorId = color.ColorId;

                var category = db.Categories
                    .Where(p => p.CategoryName == categoryName)
                    .FirstOrDefault();
                newCategoryId = category.CategoryId;

                var section = db.Sections
                    .Where(p => p.SectionName == sectionName)
                    .FirstOrDefault();
                newSectionId = section.SectionId;

               
                // Подгрузили списки  характеристик категории 
                categoryCharacteristics = await db.CategoryCharacteristics
                    .Include(h => h.Characteristic)
                    .Where(p => p.CategoryId == newCategoryId)
                    .OrderBy(h => h.OrdinationNumber)
                    .ToListAsync();
                bindingModel.CategoryCharacteristicsList = categoryCharacteristics;
               // db.CategoryCharacteristics.RemoveRange(categoryCharacteristics);


                //Список значений для ввода выбранных значений
                var inputCharacteristicValues = db.CharacteristicValues
                   .Include(p => p.Value)
                   .Where(p => p.Value.ValueName == "--")
                   .ToList();
                bindingModel.InputCharacteristicValues = inputCharacteristicValues;
               // db.CharacteristicValues.RemoveRange(inputCharacteristicValues);


                //Список значений для выпадающих списков значений в характеристиках
                //и для передачи CharacteristicValuesId выбранных значений в следующий метод действия CreateProduct ///
                allCharacteristicValues = db.CharacteristicValues
               .Include(p => p.CategoryCharacteristic)
               .Include(h => h.Value)
               .Where(p => p.CategoryCharacteristic.CategoryId == newCategoryId)
               .ToList();
                

            }
            bindingModel.AllCharacteristicValues = allCharacteristicValues;
            
            //Цель создания этого массива - получение массива списков объектов List<SelectListItem>[]
            //для выпадающих меню для задания индивидуальных характеристик товара в админке
            List<CharacteristicValue>[] characteristicValuesList = new List<CharacteristicValue>[allCharacteristicValues.Count];
                //characteristicValuesList[0] список для 1-й хар-ки товара
                //characteristicValuesList[1] список для 2-й хар-ки товара...

                for (int i = 0; i < allCharacteristicValues.Count; i++) // проход по значениям характеристики
                {
                    //список значений для i-ой характеристики        //список значений, возможных в категории
                    characteristicValuesList[i] = allCharacteristicValues
                          .Where(p => p.CategoryCharacteristic.OrdinationNumber == i + 1)
                          .ToList();
                }

                //Массив списков значений
                List<SelectListItem>[] optionCharacteristicValuesList = new List<SelectListItem>[characteristicValuesList.Length];

                for (int i = 0; i < characteristicValuesList.Length; i++)
                {
                    optionCharacteristicValuesList[i] = new List<SelectListItem>();

                    foreach (var value in characteristicValuesList[i])
                    {
                        optionCharacteristicValuesList[i].Add(new SelectListItem(value.Value.ValueName, value.Value.ValueName));
                    }
                }


                //Запоминаем выпадающие списки значений характеристик категории
                bindingModel.OptionCharacteristicValuesList = optionCharacteristicValuesList;


            newColoredProduct.Product.Description = "Описание товара...";
            newColoredProduct.ShowProduct = true;
            bindingModel.ColoredProduct = newColoredProduct;

            bindingModel.ProductCode = newColoredProduct.ProductCode;
            bindingModel.ProductName = newColoredProduct.ProductName;
            bindingModel.Price = newColoredProduct.Price;
            bindingModel.ShowProduct = newColoredProduct.ShowProduct;
            bindingModel.Stock = newColoredProduct.Stock;

            bindingModel.NewColorId = newColorId;
            bindingModel.NewCategoryId = newCategoryId;
            bindingModel.NewSectionId = newSectionId;

            return bindingModel;
        }


        //Запись в БД данных формы создания товара
        public async Task<ProductWithMessage> CreateProduct(CreateProductBindingModel bm, bool modelState, int[] categoryCharacteristicsIds, string[] valueNames)
        {
            ProductWithMessage productWithMessage = new ProductWithMessage();

            using (MyStoreContext db = new MyStoreContext())
            {

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Product
                        var newProduct = new Product()
                        {
                            ProductId = 0,
                            CategoryId = bm.NewCategoryId,
                            SectionId = bm.NewSectionId,
                            Description = bm.ColoredProduct.Product.Description
                        };
                        await db.Products.AddAsync(newProduct);
                        await db.SaveChangesAsync();

                        //Восполняем Id
                        newProduct.ProductId = db.Products
                            .Max(p => p.ProductId);

                        productWithMessage.ProductId = newProduct.ProductId;

                        //Извлекаем картинки
                        byte[] image = null;
                        if (modelState)
                        {
                            List<Image> images = new List<Image>();
                            images = GetFiles(bm.Files, newProduct.ProductId, bm.NewColorId);
                            await db.Images.AddRangeAsync(images);

                            if (images.Count != 0)
                            {
                                image = images[0].ImageBody;
                            }
                        }
                        
                        if (bm./*ColoredProduct.*/ProductCode == null)
                        {
                            bm./*ColoredProduct.*/ProductCode = "NEW-" + Guid.NewGuid().ToString();
                        }
                        else
                        {
                            //Проверка, есть ли уже такой ProductCode в БД
                            var coloredProduct = db.ColoredProducts
                               // .Select(p => new { p.ProductCode })
                                .Where(p => p.ProductCode == bm./*ColoredProduct.*/ProductCode);

                            if (coloredProduct != null )
                            {
                                bm./*ColoredProduct.*/ProductCode = bm./*ColoredProduct.*/ProductCode + "-" + Guid.NewGuid().ToString();
                            }
                        }
                        //ColoredProduct
                        var newColoredProduct = new ColoredProduct()
                        {
                            ProductId = newProduct.ProductId,
                            ColorId = bm.NewColorId,
                            ProductCode = bm./*ColoredProduct.*/ProductCode,
                            ProductName = bm./*ColoredProduct.*/ProductName,
                            Price = bm./*ColoredProduct.*/Price,
                            ImgResized = image,
                            Stock = bm./*ColoredProduct.*/Stock,
                            ShowProduct = bm./*ColoredProduct.*/ShowProduct
                        };

                        await db.ColoredProducts.AddAsync(newColoredProduct);

                        //ProductValues
                        List<ProductValue> newProductValues = new List<ProductValue>();

                        var allCharacteristicValues = db.CharacteristicValues
                               .Include(p => p.CategoryCharacteristic)
                               .Include(p => p.Value)
                               .Select(p => new { p.CategoryCharacteristicsId, p.CategoryCharacteristic.CategoryId, p.CharacteristicValuesId, p.Value.ValueName })
                               .Where(p => p.CategoryId == bm.NewCategoryId)
                               .ToList();

                        for (int i = 0; i < categoryCharacteristicsIds.Length; i++)
                        {
                            var characteristicValue = allCharacteristicValues
                                .Where(p => p.CategoryCharacteristicsId == categoryCharacteristicsIds[i])
                                .Where(p => p.ValueName == valueNames[i])
                                .FirstOrDefault();

                           newProductValues.Add(new ProductValue() { CharacteristicValuesId = characteristicValue.CharacteristicValuesId, ProductId = newProduct.ProductId });
                        }
                        
                        await db.ProductValues.AddRangeAsync(newProductValues);

                        await db.SaveChangesAsync();

                        transaction.Commit();
                        productWithMessage.Message = $"Новый товар {newColoredProduct.ProductCode} успешно создан";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        productWithMessage.Message = ($"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ");
                    }
                }
            }
            productWithMessage.ColorId = bm.NewColorId;
            
            

            return productWithMessage;
        }

        public List<Image> GetFiles(List<IFormFile> files, int productId, int colorId)
        {
            List<Image> images = new List<Image>();

            foreach (var file in files)
            {
                //Создаём массив байт и заполняем его из file
                byte[] imageBody = null;

                using (BinaryReader b = new BinaryReader(file.OpenReadStream())) // .OpenReadStream() выдаёт поток байт файла из зароса
                {
                    imageBody = b.ReadBytes((int)file.Length);// на экземпляре с потоком  вызываем метод получения байтового массива
                                                              // .ReadBytes(..) Считывает указанное количество байтов из текущего потока в байтовый массив и перемещает текущую позицию на это количество байтов вперед. 
                }

                //Находим имя файла без пути
                var fileName = Path.GetFileName(file.FileName);

                //делаем измененное имя файла
                var changedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileName)); //имя файла разбили на массив байт и отправили в .ToBase64String(..) 

                //Составляем список картинок
                images.Add(new Image() { ProductId = productId, ColorId = colorId, ImageBody = imageBody, ChangedName = changedName });
            }
            return images;
        }

        public async Task AddFilesToDatabase(List<IFormFile> files, int productId, int colorId)
        {
            List<Image> images = GetFiles(files, productId, colorId);

            using (MyStoreContext db = new MyStoreContext())
            {
                db.Images.AddRange(images);

                //Грузим соответствующую строку в ColoredProducts и добавляем в нее картинку
                var coloredProduct = db.ColoredProducts
                        .Where(p => p.ProductId == productId)
                        .Where(p => p.ColorId == colorId)
                        .FirstOrDefault();
                coloredProduct.ImgResized = images[0].ImageBody;

                db.ColoredProducts.Attach(coloredProduct);
                //db.Images.AttachRange(images);


                await db.SaveChangesAsync();
            }
        }
    } 
}

