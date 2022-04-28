using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure.Attributes
{
    public class ExistClientsEmailAttribute : ValidationAttribute
    {  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string)) return new ValidationResult("This attribute can only be used on a string type");

            using (MyStoreContext db = new MyStoreContext())
            {
                var oldClient = db.Clients
                    .Select(p => new { p.Email })
                    .Where(p => p.Email == value.ToString() )
                    .FirstOrDefault();

                if (oldClient == null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Пользователь с таким Email уже зарегистрирован");
                }
            }
        }
    }
}
