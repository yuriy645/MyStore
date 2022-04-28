using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyStore
{
    public partial class Section
    {
        public Section()
        {
            Products = new HashSet<Product>();
        }

        public int SectionId { get; set; }

        [Display(Name = "Раздел")]
        public string SectionName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
