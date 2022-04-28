using MyStore.Infrastructure;
using MyStore.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MyStore
{
    public partial class ColoredProduct
    {
        public ColoredProduct()
        {
            Images = new HashSet<Image>();
            Purchases = new HashSet<Purchase>();
        }
        public virtual ICollection<Image> Images { get; set; }

        public int ProductId { get; set; }
        public int ColorId { get; set; }


        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        public bool ShowProduct { get; set; }

        public byte[] ImgResized { get; set; }

        
        public int Stock { get; set; }

        

        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }

        [NotMapped]
        [Int]
        public int CartQuantity { get; set; }
    }
}
