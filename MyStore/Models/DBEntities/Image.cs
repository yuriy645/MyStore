using Microsoft.AspNetCore.Http;
using MyStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MyStore
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public byte[] ImageBody { get; set; }
        public string ChangedName { get; set; }
        public int? ImageNumber { get; set; }

        public virtual ColoredProduct ColoredProduct { get; set; }

        

        
    }
}
