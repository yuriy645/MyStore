using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyStore
{
    public partial class DeliveryType
    {
        public DeliveryType()
        {
            Orders = new HashSet<Order>();
        }
        public int DeliveryTypeId { get; set; }

        [Display(Name = "Способ доставки")]
        public string DeliveryTypeName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
