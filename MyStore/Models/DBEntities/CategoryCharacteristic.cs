using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyStore
{
    public partial class CategoryCharacteristic
    {
        [UIHint("HiddenInput")]
        public int CategoryCharacteristicsId { get; set; }
        public int CategoryId { get; set; }
        public int CharacteristicId { get; set; }
        public int OrdinationNumber { get; set; }

        public virtual Category Category { get; set; }
        public virtual Characteristic Characteristic { get; set; }
        public virtual List<CharacteristicValue> CharacteristicValues { get; set; }
    }
}
