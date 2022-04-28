using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore
{
    public partial class ProductValue
    {
        public int ProductValuesId { get; set; }
        public int CharacteristicValuesId { get; set; }
        public int ProductId { get; set; }

        public virtual CharacteristicValue CharacteristicValue { get; set; }//**
       // public virtual CharacteristicValue CategoryCharacteristic { get; set; }//**
        public virtual Product Product { get; set; }
    }
}
