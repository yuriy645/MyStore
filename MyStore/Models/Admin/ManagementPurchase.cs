using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Admin
{
    public class ManagementPurchase
    {
        public string ProductCode { get; set; }
        public byte[] ImgResized { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
