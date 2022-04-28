using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore
{
    public partial class Value
    {
        public Value()
        {
            CharacteristicValues = new HashSet<CharacteristicValue>();
        }
        public int ValueId { get; set; }
        public string ValueName { get; set; }
        public virtual ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    }
}
