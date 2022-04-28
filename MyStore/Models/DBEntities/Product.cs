using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyStore
{
    public partial class Product
    {
        public Product()
        {
            ColoredProducts = new HashSet<ColoredProduct>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SectionId { get; set; }

        [UIHint("MultilineText")]
        public string Description { get; set; }

        public virtual Category Category { get; set; }
        public virtual Section Section { get; set; }
        public virtual ICollection<ColoredProduct> ColoredProducts { get; set; }
        public virtual ICollection<ProductValue> ProductValues { get; set; }
    }
}
