using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MyStore
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }
        public int ClientId { get; set; }          //Not Null

        public DateTime RegisterDate { get; set; } //Not Null

        public string Email { get; set; }          //Not Null
        public string PassHash { get; set; }       //Not Null
        
        public long Phone { get; set; }            //Not Null

        public string DeliveryMeth { get; set; }

        public string Name { get; set; }            //Not Null
        public string SecondName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartament { get; set; }
        public int? UkrIndex { get; set; }
        public int? Npnumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        [NotMapped]
        public Order Order { get; set; }
    }
}
