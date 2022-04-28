using MyStore.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Home.BindingModels
{
    public class RegistrationClientBindingModel
    {
        [Required(ErrorMessage = "Укажите Email пользователя")]
        [ExistClientsEmail]         //проверка, есть ли такая почта в БД в табл Clients
        [Display(Name = "Email *")] //подпись поля
        [EmailAddress]              //проверка, является ли поле почтой
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

        [Required(ErrorMessage = "Укажите номер телефона")]
        //[Phone]                       //проверка, является ли поле телефоном
        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Телефон *")]
        public long Phone { get; set; }

        [Display(Name = "Способ доставки")]
        public string DeliveryMeth { get; set; }

        [Required(ErrorMessage = "Укажите имя пользователя")]
        [Display(Name = "Имя *")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Display(Name = "Дом")]
        public string House { get; set; }

        [Display(Name = "Квартира/ Офис")]
        public string Apartament { get; set; }

        [Display(Name = "Почтовый индекс")]
        public int? UkrIndex { get; set; }

        [Display(Name = "Номер отделения новой почты")]
        public int? Npnumber { get; set; }

        public string Message { get; set; }

    }
}
