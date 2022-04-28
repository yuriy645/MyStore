using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Admin
{
    public class ManagementOrder
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }        
        public string ClientEmail { get; set; }
        public string DeliveryTypeName { get; set; }
        public string Comment { get; set; }
        public string AdminComment { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSecondName { get; set; }

        [UIHint("Boolean")]
        public bool Completed { get; set; }

        public List<ManagementPurchase> ManagementPurchases { get; set; }
    }
}
