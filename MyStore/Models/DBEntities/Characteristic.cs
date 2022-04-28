using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore
{
    public partial class Characteristic
    {
        public Characteristic()
        {
            CategoryCharacteristics = new HashSet<CategoryCharacteristic>();
        }

        public int CharacteristicId { get; set; }
        public string CharacteristicName { get; set; }

        public virtual ICollection<CategoryCharacteristic> CategoryCharacteristics { get; set; }
    }
}
