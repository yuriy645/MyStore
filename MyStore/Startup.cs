using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Models.Home;
using MyStore.Infrastructure;
using MyStore.Models.Store;

namespace MyStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSession(options =>
            {
                options.Cookie.Name = "Client.Session"; //íàçâàíèå êóê âìåñòî äåôîëòíîãî ".AspNet.Session"
                options.IdleTimeout = TimeSpan.FromMinutes(5); //âðåìÿ æèçíè ñåññèè ñ ó÷åòîì àêòèâíîñòè ïîëüçîâàòåëÿ
                options.Cookie.IsEssential = true; //true - êóêè êðèòè÷íû è íåîáõîäèìû äëÿ ðàáîòû ýòîãî ïðèëîæåíèÿ
            });
            services.AddMemoryCache();

            services.AddTransient<IODatabase>();
            services.AddTransient<IOHome>(); 
            services.AddTransient<IOStore>();
            services.AddTransient<Authorisation>(); 
            services.AddTransient<CartService>(); 
            services.AddTransient<LogReaderService>();
            //        services.Configure<MvcViewOptions>(options =>
            //options.HtmlHelperOptions.CheckBoxHiddenInputRenderMode =
            //    CheckBoxHiddenInputRenderMode.None);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Store}/{action=Index}/{section}/{category}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Store}/{action=Cart}");

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Store}/{action=CartForm}");

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Admin}/{action=DeleteImage}/{productId}/{colorId}/{imageId}");

                endpoints.MapControllerRoute(
                    name: "default1",
                    pattern: "{controller=Admin}/{action=ProductsManagement}");


            });
        }
    }
}//
