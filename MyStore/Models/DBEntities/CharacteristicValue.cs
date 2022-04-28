using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore
{
    public partial class CharacteristicValue
    {
        public CharacteristicValue()//**
        {
            ProductValues = new HashSet<ProductValue>();
        }
        public int CharacteristicValuesId { get; set; }
        public int CategoryCharacteristicsId { get; set; }
        public int ValueId { get; set; }

        //public virtual CategoryCharacteristic CharacteristicValues { get; set; }**
        public virtual CategoryCharacteristic CategoryCharacteristic { get; set; }//**
        public virtual Value Value { get; set; }
        public virtual ICollection<ProductValue> ProductValues { get; set; }
    }
}
