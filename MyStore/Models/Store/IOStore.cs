using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyStore.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Store
{
    public class IOStore
    {
        
        //Сохраняет заказ в БД
        public async Task<string> SendOrderToDB(Client inClient)
        {
            string message = null;

            using (MyStoreContext db = new MyStoreContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Пытаемся вытянуть из БД входящего клиента
                        var clientOld = await db.Clients
                            .Select(p => new { p.Email, p.ClientId })
                            .Where(p => p.Email == inClient.Email)
                            .FirstOrDefaultAsync();

                        //Если нашелся, то берем Id записи
                        if (clientOld != null)
                        {
                            inClient.ClientId = clientOld.ClientId;
                        }
                        else //Если не нашелся, 
                        {
                            //Client clientNew = new()
                            //{ 
                            //    RegisterDate = inClient.RegisterDate, 
                            //    Email = inClient.Email,
                            //    PassHash = inClient.PassHash,
                            //    Phone = inClient.Phone,
                            //    Name = inClient.Name
                            //};

                            //то входящий клиент -автосозданный, добавим его в БД
                            
                            db.Clients.Add(inClient/*clientNew*/);
                            await db.SaveChangesAsync();

                            //и вытянем, чтоб узнать Id
                            var clientNew = await db.Clients
                            .Select(p => new { p.Email, p.ClientId })
                            .Where(p => p.Email == inClient.Email)
                            .FirstOrDefaultAsync();

                            inClient.ClientId = clientNew.ClientId;
                        }

                        //Находим Id способа доставки
                        var deliveryTypeNew = await db.DeliveryTypes
                            .Select(p => new { p.DeliveryTypeName, p.DeliveryTypeId })
                            .SingleOrDefaultAsync(p => p.DeliveryTypeName == inClient.Order.DeliveryType.DeliveryTypeName);

                        Order orderNew = new Order()
                        {
                            ClientId = inClient.ClientId,
                            OrderDate = DateTime.Now,
                            Comment = inClient.Order.Comment,
                            Summ = inClient.Order.Summ,
                            EmployeeId = 1 //Сотрудник будет выбираться в админке, тут пока ставим Id пустого сотрудника
                        };

                        //inClient.Order.ClientId = inClient.ClientId;
                        //inClient.Order.OrderDate = DateTime.Now;
                        //inClient.Order.EmployeeId = 1; 

                        if (deliveryTypeNew != null) //проверка на всякий случай
                        {
                           orderNew.DeliveryTypeId = deliveryTypeNew.DeliveryTypeId;
                           // inClient.Order.DeliveryTypeId = deliveryTypeNew.DeliveryTypeId;
                        }
                        //Обновление ClientId и внесение заказа в БД
                        //inClient.Order.ClientId = inClient.ClientId;

                        db.Orders.Add(/*inClient.Order*/ orderNew);
                        await db.SaveChangesAsync();

                        //Выясняем Id нового заказа
                        orderNew.OrderId = await db.Orders
                            .Select(p => new { p.OrderId })
                            .MaxAsync(p => p.OrderId);

                        List<Purchase> purchases = new List<Purchase>();
                        //Добавляем OrderId ко всем заказанным товарам
                        foreach (var purchase in inClient.Order.Purchases)
                        {
                            purchases.Add(new Purchase()
                            {
                                ProductId = purchase.ProductId,
                                ColorId = purchase.ColorId,
                                Quantity = purchase.Quantity,
                                OrderId = orderNew.OrderId
                            });
                        }

                        db.Purchases.AddRange(purchases);
                        await db.SaveChangesAsync();

                        message = "Спасибо за покупку";
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        message = $"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ";
                    }
                }
            }
            return message;
        }

        //Формирует данные для выпадающего списка со способами доставки
        public async Task<DeliveryTypesModel> GetDeliveryTypesModel()
        {
            DeliveryTypesModel deliveryTypesModel = new DeliveryTypesModel();

            List<DeliveryType> deliveryTypes = new List<DeliveryType>();

            deliveryTypesModel.DeliveryTypeNameOptions = new List<SelectListItem>();

            using (MyStoreContext db = new MyStoreContext())
            {
                deliveryTypes = await db.DeliveryTypes
                       .ToListAsync();
            }

            foreach (var deliveryType in deliveryTypes)
            {
                deliveryTypesModel.DeliveryTypeNameOptions.Add(new SelectListItem(deliveryType.DeliveryTypeName, deliveryType.DeliveryTypeName));
            }

            //deliveryTypesModel.SelectedDeliveryTypeName = "-";

            return deliveryTypesModel;
        }
    }
}
