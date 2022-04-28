using MyStore.Models.Home.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Store
{
    public class StoreLayoutBindingModel
    {
        public SignInBindingModel SignInBindingModel { get; set; }
        public List<ColoredProduct> ColoredProducts { get; set; }
        public ColoredProduct ColoredProduct { get; set; }
        public RegistrationClientBindingModel RegistrationClientBindingModel { get; set; }
        public RegistrationEmployeeBindingModel RegistrationEmployeeBindingModel { get; set; }
        //public StoreClient StoreClient { get; set; }
        public Client Client { get; set; }
        public DeliveryTypesModel DeliveryTypesModel { get; set; }
        public string Message { get; set; }
    }
}
