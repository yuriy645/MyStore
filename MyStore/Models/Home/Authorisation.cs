using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStore;
using MyStore.Models.Home.BindingModels;
using MyStore.Infrastructure;

namespace MyStore.Models.Home
{
    public class Authorisation
    {

        IOHome _iOHome;
        public Authorisation(IOHome _iOHome)//тут сам создается экземпляр ClientDB
        {
            this._iOHome = _iOHome;
        }
        public async Task<string> TryAuthorize(SignInBindingModel inUser, HttpContext httpContext)
        {
            string authorisationMessage = null;
            SignInBindingModel inUserWithoutHash = new SignInBindingModel();
            inUserWithoutHash.Email = inUser.Email;
            inUserWithoutHash.Pass = inUser.Pass;

            inUser.Pass = PasswordHasher.HashPassword(inUser.Pass);

            Client client = await _iOHome.GetClientFromDB(inUser); //клиент по хэш коду

            //Для совместимости с предзаполненной БД пробуем подставлять пароль напрямую.
            if (client == null)
            {
                client = await _iOHome.GetClientFromDB(inUserWithoutHash);
            }

            if (client != null) // если новый клиент не появился
            {
                httpContext.Session.Set<Employee>("authorizedEmployee", null);
                httpContext.Session.Set<Client>("authorizedClient", client);
                authorisationMessage = $"Здравствуйте, {client.Name}";
            }
            else
            {
                Employee employee = await _iOHome.GetEmployeeFromDB(inUser);

                //Для совместимости с предзаполненной БД пробуем подставлять пароль напрямую.
                if (employee == null)
                {
                    employee = await _iOHome.GetEmployeeFromDB(inUserWithoutHash);
                }

                if (employee !=null)
                {
                    httpContext.Session.Set<Client>("authorizedClient", null);
                    httpContext.Session.Set<Employee>("authorizedEmployee", employee);
                    authorisationMessage = $"Здравствуйте, {employee.Name}";
                }
                else
                {
                    authorisationMessage = "Пользователь с таким логином и паролем не найден"; // на странице Index

                    //пробуем работать с прежним клиентом
                    //var authorizedClient = httpContext.Session.Get<Client>("authorizedClient");//получаем клиента из сессии
                    //if (authorizedClient != null)
                    //{
                    //    client = authorizedClient;
                    //}
                }
            }
            

            return authorisationMessage;
        }
    }
}
