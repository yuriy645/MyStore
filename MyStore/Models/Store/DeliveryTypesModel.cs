using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Store
{
    public class DeliveryTypesModel
    {
        //public string SelectedDeliveryTypeName { get; set; }
        public List<SelectListItem> DeliveryTypeNameOptions { get; set; }
    }
}
