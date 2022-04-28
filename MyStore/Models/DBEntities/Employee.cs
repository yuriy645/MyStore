using System;
using System.Collections.Generic;

#nullable disable

namespace MyStore
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
        }
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string PassHash { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
