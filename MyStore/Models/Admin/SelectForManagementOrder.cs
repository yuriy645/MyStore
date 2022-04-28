using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Admin
{
    public class SelectForManagementOrder
    {
        public Order order { get; set; }
        public List<ManagementOrder> ManagementOrders { get; set; }
        public int count { get; set; }
    }
}
