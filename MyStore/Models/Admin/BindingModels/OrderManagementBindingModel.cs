using MyStore.Models.Admin;
using MyStore.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.StoreEntities
{
    public class OrderManagementBindingModel
    {
        //public List<Purchase> Purchases { get; set; }
        public List<ManagementOrder> ManagementOrders { get; set; }

        [Display(Name = "Показывать все заказы/ только не выполненные")]
        [UIHint("Boolean")]
        public bool ShowAllOrders { get; set; }

        public Client Client { get; set; }

        public string AdminComment { get; set; }
        public bool Completed { get; set; }
        public int OrderId { get; set; }

        //public List<Clas> b { get; set; }

    }
}
