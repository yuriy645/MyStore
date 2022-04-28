using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.DBEntities
{
    public class Purchase
    {
        [UIHint("HiddenInput")]
        public int OrderId { get; set; }

        [UIHint("HiddenInput")]
        public int ProductId { get; set; }

        [UIHint("HiddenInput")]
        public int ColorId { get; set; }
        public int Quantity { get; set; }

        public virtual ColoredProduct ColoredProduct { get; set; }
        public virtual Order Order { get; set; }
    }
}
