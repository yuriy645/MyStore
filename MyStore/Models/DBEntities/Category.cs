using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyStore
{
    public partial class Category
    {
        public Category()
        {
            CategoryCharacteristics = new HashSet<CategoryCharacteristic>();
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }

        [Display(Name = "Категория")]
        public string CategoryName { get; set; }

        public virtual ICollection<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
