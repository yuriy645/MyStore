using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure.Attributes
{
    public class ExistEmployeeEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string)) return new ValidationResult("This attribute can only be used on a string type");

            using (MyStoreContext db = new MyStoreContext())
            {
                var oldEmployee = db.Employees
                    .Select(p => new { p.Email })
                    .Where(p => p.Email == value.ToString())
                    .FirstOrDefault();

                if (oldEmployee == null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Сотрудник с таким Email уже зарегистрирован");
                }
            }
        }
    }
}
