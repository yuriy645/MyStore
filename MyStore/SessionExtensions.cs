using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MyStore
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        //public static StoreClient ConvertToStoreClient(Client client)
        //{
        //    StoreClient newStoreClient = new()
        //    {
        //        RegisterDate = client.RegisterDate,
        //        Email = client.Email,
        //        PassHash = client.PassHash,
        //        Phone = client.Phone,
        //        Name = client.Name,
        //        DeliveryMeth = client.DeliveryMeth,
        //        SecondName = client.SecondName,
        //        City = client.City,
        //        Street = client.Street,
        //        House = client.House,
        //        Apartament = client.Apartament,
        //        UkrIndex = client.UkrIndex,
        //        Npnumber = client.Npnumber
        //    };

        //    //newStoreClient.StoreOrders = new List<Orders>()

        //    return newStoreClient;
        //}


        //отправляет в сессию настройки авторизации: данные клиента и цвет футера
        //public static void GetClientToSession(this Controller controller, Client client)
        //{
        //    controller.HttpContext.Session.Set<Client>("authorizedClient", client); //отправляем в сессию
        //}

        //    public Image ResizeOrigImg(Image image, int nWidth, int nHeight)
        //    {
        //        int newWidth, newHeight;
        //        var coefH = (double)nHeight / (double)image.Height;
        //        var coefW = (double)nWidth / (double)image.Width;
        //        if (coefW >= coefH)
        //        {
        //            newHeight = (int)(image.Height * coefH);
        //            newWidth = (int)(image.Width * coefH);
        //        }
        //        else
        //        {
        //            newHeight = (int)(image.Height * coefW);
        //            newWidth = (int)(image.Width * coefW);
        //        }

        //        Image result = new Bitmap(newWidth, newHeight);
        //        using (var g = Graphics.FromImage(result))
        //        {
        //            g.CompositingQuality = CompositingQuality.HighQuality;
        //            g.SmoothingMode = SmoothingMode.HighQuality;
        //            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //            g.DrawImage(image, 0, 0, newWidth, newHeight);
        //            g.Dispose();
        //        }
        //        return result;
        //    }
    }
}
