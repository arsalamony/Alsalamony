using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public int AddressId { get; set; }


        public Address? Address { get; set; }
    }
}
