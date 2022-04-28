using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore
{
    public partial class Color
    {
        public Color()
        {
            ColoredProducts = new HashSet<ColoredProduct>();
        }

        public int ColorId { get; set; }
        public string ColorName { get; set; }

        public virtual ICollection<ColoredProduct> ColoredProducts { get; set; }
    }
}
