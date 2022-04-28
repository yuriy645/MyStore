using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class DashAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if ( (value != null) && (value.ToString() == "-") )
            {
                return new ValidationResult("Выберите корректное значение из списка");

            }
            else

                return ValidationResult.Success;
        }
    }
}
