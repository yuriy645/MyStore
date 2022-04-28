using MyStore.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Home.BindingModels
{
    public class RegistrationEmployeeBindingModel
    {
        [Required(ErrorMessage = "Укажите Email сотрудника")]
        [ExistEmployeeEmail]
        [Display(Name = "Email *")] 
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль *")]
        [UIHint("Password")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть от 3-х до 20 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите подтверждение пароля")]
        [Display(Name = "Подтверждение пароля *")]
        [UIHint("Password")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Укажите имя сотррудника")]
        [Display(Name = "Имя *")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите фамилию сотрудника")]
        [Display(Name = "Фамилия *")]
        public string SecondName { get; set; }

        public string Message { get; set; }
    }
}
