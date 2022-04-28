using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Infrastructure
{
    public class IntAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is int) 
            {
                return ValidationResult.Success;
            }
            else
                return new ValidationResult("Укажите целое число");

        }
    }
}
