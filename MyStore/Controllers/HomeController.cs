using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyStore.Models;
using MyStore.Models.Home;
using MyStore.Models.Home.BindingModels;
using MyStore.Models.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyStore;
using MyStore.Infrastructure;

namespace MyStore.Controllers
{
    [LogTime]
    public class HomeController : Controller
    {
        private readonly Authorisation _authorisation;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Authorisation authorisation)
        {
            _logger = logger;
            _authorisation = authorisation;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(StoreLayoutBindingModel storeLayoutBindModel)
        {
            storeLayoutBindModel.SignInBindingModel.AuthorisationMessage = await _authorisation.TryAuthorize(storeLayoutBindModel.SignInBindingModel, this.HttpContext);
            return View("Index", storeLayoutBindModel);
        }

        

        public IActionResult RegistrationForm()
        {
            //Если в сессии есть авторизованный сотрудник, то показывать форму регистрации сотрудника
            var authorizedEmployee = HttpContext.Session.Get<Employee>("authorizedEmployee");//получаем сотрудника из сессии
            if (authorizedEmployee != null)
            {
                return View("RegistrationFormEmployee"); 
            }
            else
            {
                return View("RegistrationFormClient");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee([FromServices] IOHome _iOHome, StoreLayoutBindingModel bm)
        {

             var bmNew = await _iOHome.RegEmployee(bm.RegistrationEmployeeBindingModel);

            StoreLayoutBindingModel sLbm = new StoreLayoutBindingModel();
            sLbm.RegistrationEmployeeBindingModel = bmNew;

            //if (ModelState.IsValid)
            //{
            //    return View("Success");
            //}
            //else
            //{
            //    Debug.WriteLine($"Ошибка регистрации");
            //    return View(model);
            //}

            return View("RegistrationFormEmployee", sLbm);

        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient([FromServices] IOHome _iOHome, StoreLayoutBindingModel bm)
        {

            if (ModelState["RegistrationClientBindingModel.Email"].Errors.Count == 0) //Проверка одного свойства модели
            {
                // Внесёт клиента в БД и прикрепит сообщение
                bm.RegistrationClientBindingModel = await _iOHome.RegClient(bm.RegistrationClientBindingModel);
            }

            return View("RegistrationFormClient", bm);
            
        }

        public IActionResult NotEmployee()
        {
            return View();
        }

        public IActionResult GetOut()
        {
            HttpContext.Session.Set<Client>("authorizedClient", null);
            HttpContext.Session.Set<Employee>("authorizedEmployee", null);
            return View("Index");
        }

        public IActionResult Articles()
        {
            return View();
        }

        public IActionResult Questions()
        {
            return View();
        }

        public IActionResult Prices()
        {
            return View();
        }

        public IActionResult Delivery()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}
