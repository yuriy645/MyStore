using MyStore.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyStore
{
    public partial class Order
    {
        public Order()
        {
            Purchases = new List<Purchase>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ClientId { get; set; }
        
        [Display(Name = "Комментарий")]
        [UIHint("MultilineText")]
        public string Comment { get; set; }
        public string AdminComment { get; set; }
        public int EmployeeId { get; set; }
        public int DeliveryTypeId { get; set; }

        [UIHint("HiddenInput")]
        public decimal Summ { get; set; }

        public bool Completed { get; set; }

        public virtual Client Client { get; set; }
        public /*virtual*/ DeliveryType DeliveryType { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual List<Purchase> Purchases { get; set; }
    }
}
