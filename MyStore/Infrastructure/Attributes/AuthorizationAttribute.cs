using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
        public class AuthorizationAttribute : Attribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (context.HttpContext.Session.Get<Employee>("authorizedEmployee") == null)
                {
                    /*Если запрос удовлетворяет политике авторизации, то фильтр авторизации ничего
                    не делает; такая пассивность позволяет инфраструктуре МVС перейти к следующему
                    фильтру и в конечном итоге выполнить метод действия.
                    */
                    context.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                            (
                                new
                                {
                                    controller = "Home",
                                    action = "NotEmployee"//,
                                                            //area = "AreaName"
                                }
                            )
                    );

                    //new ViewResult { ViewName = "~/Views/Shared/NotRegistered.cshtml"};
                    // установка значения Result контекста позволяет на любом этапе обработки фильтров выйти из этого конвейера,
                    // отменив выполнение последующих фильтров
                }
            }
        }
    
}
