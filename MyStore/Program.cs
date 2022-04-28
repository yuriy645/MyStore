using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // При использовании ASP.NET Core User Secrets добавляются автоматически при запуске проекта в Development окружении
            // Для редактирования секретов текущего приложения используйте пункт контекстного меню Manage User Secret на текущем проекте
            Host.CreateDefaultBuilder(args)

            .ConfigureAppConfiguration(config =>
            {
                config.AddEnvironmentVariables();//поключение работы с переменными окружения
                                                 // конфигурации находятся в csproj файле
                                                 // пример явного определения секретов для текущей конфигурации
                                                 //при разработке удобно использовать другой механизм - пользовательские секреты удобно использовать при разработке
                                                 //config.AddUserSecrets("ce0b4330-4258-4bf4-b9aa-2891772e47cc"); 
            })
             .ConfigureAppConfiguration(builder =>
             {
                 builder.AddJsonFile("appsettings.json", false, true);
             })
            //Whether the file is optional.
            //Whether the configuration should be reloaded if the file changes.

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        

    }
}
