using Microsoft.EntityFrameworkCore;
using MyStore.Infrastructure;
using MyStore.Models.Home.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Home
{
    public class IOHome
    {
        public async Task<Client> GetClientFromDB(SignInBindingModel bindModel)
        {
            MyStoreContext db = new MyStoreContext();
            
                Client client = await db.Clients
                       .Where(b => b.Email == bindModel.Email)
                       .Where(b => b.PassHash == bindModel.Pass)
                       .FirstOrDefaultAsync();
                return client;
        }

        public async Task<Employee> GetEmployeeFromDB(SignInBindingModel bindModel)
        {
            MyStoreContext db = new MyStoreContext();

            Employee employee = await db.Employees
                   .Where(b => b.Email == bindModel.Email)
                   .Where(b => b.PassHash == bindModel.Pass)
                   .FirstOrDefaultAsync();
            return employee;
        }

        public async Task<RegistrationClientBindingModel> RegClient(RegistrationClientBindingModel bm)
        {
            //if (!ModelState.IsValid)
            {

            }
            var client = new Client()
            {
                RegisterDate = DateTime.Now,
                Email = bm.Email, 
                PassHash = PasswordHasher.HashPassword(bm.Password), 
                Phone = bm.Phone,
                DeliveryMeth = bm.DeliveryMeth,
                Name = bm.Name,
                SecondName = bm.SecondName,
                City = bm.City,
                Street = bm.Street,
                House = bm.House,
                Apartament = bm.Apartament,
                UkrIndex = bm.UkrIndex,
                Npnumber = bm.Npnumber
            };

            try
            {
                using (MyStoreContext db = new MyStoreContext())
                {
                    // Проверка будет в атрибуте
                    //var oldClient = db.Clients
                    //    .Select(p => new { p.Email })
                    //    .Where(p => p.Email == bm.Email)
                    //    .FirstOrDefault();

                    //if (oldClient == null)
                    //{
                        await db.Clients.AddAsync(client);
                        await db.SaveChangesAsync();
                        bm.Message = "Регистрация успешна";
                    //}
                    //else 
                    //{
                    //    bm.Message = "Пользователь с таким Email уже зарегистрирован";
                    //}  
                }
                
            }
            catch (Exception ex)
            {
                bm.Message = $"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ";
            }

            return bm;
        }

        public async Task<RegistrationEmployeeBindingModel> RegEmployee(RegistrationEmployeeBindingModel bm)
        {
            var employee = new Employee()
            {
                Email = bm.Email,
                PassHash = PasswordHasher.HashPassword(bm.Password),
                Name = bm.Name,
                SecondName = bm.SecondName
            };

            try
            {
                using (MyStoreContext db = new MyStoreContext())
                {
                    await db.Employees.AddAsync(employee);
                    await db.SaveChangesAsync();
                }
                bm.Message = "Регистрация сотрудника успешна";
            }
            catch (Exception ex)
            {
                bm.Message = $"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ";
            }

            return bm;
        }

    }
}
