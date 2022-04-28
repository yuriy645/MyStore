using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.StoreEntities
{
    public class ProductWithMessage
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string Message { get; set; }
    }
}
