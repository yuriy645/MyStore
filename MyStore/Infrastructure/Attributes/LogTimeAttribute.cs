using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class LogTimeAttribute : ActionFilterAttribute
    {
        //Запись в лог каждого обращения к методам действия
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var employee = context.HttpContext.Session.Get<Employee>("authorizedEmployee");
            var client = context.HttpContext.Session.Get<Client>("authorizedClient");

            string userName = context.HttpContext.User.Identity.Name; //будет находиться с пом cookies

            var headers = context.HttpContext.Request.Headers  //Headers типа IHeaderDictionary, кот насл-ся от кучи разных интерфейсов...
                                                               //значит можем использовать методы linq
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First());
            var headersString = headers.Aggregate(// вывод на основе тех пар ключ-значение
                                                  // headersString- строка, кот попадет куда-то там на экран
                new StringBuilder(Environment.NewLine), (builder, kv) => builder.Append($"{kv.Key} = {kv.Value} {Environment.NewLine}"))
                //Environment.NewLine - переход на новые строчки
                .ToString();


            string message =
                $"CALL TIME {DateTime.Now.AddHours(7).ToString("d")} {DateTime.Now.AddHours(7).ToString("T")},  " + // Call time {DateTime.Now.AddHours(-5).ToString(/*"T"*/)}, 
                $"EXECUTED METHOD {context.ActionDescriptor.DisplayName}, " +
                $"USER { (employee != null ? employee.Email : "") } { (client != null ? client.Email : "") } " +
                $"\nHeaders: {headersString}; \n" + // \nUsername: {userName}
                $"\n***";
            // до вызова делегата ActionExecutionDelegate, код который запустится перед методом действия
            await next();
            // после вызова делегата ActionExecutionDelegate, код который запустится по завершению метода действия


            using (StreamWriter streamWriter = new StreamWriter("log.txt", true))
            {
                await streamWriter.WriteLineAsync(message);
                streamWriter.Close();
            }

        }

    }
}
