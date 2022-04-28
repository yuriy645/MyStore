using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Home.BindingModels
{
    public class SignInBindingModel 
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Pass")]
        [UIHint("Password")]
        public string Pass { get; set; }

        public string AuthorisationMessage { get; set; }
        
    }
}
