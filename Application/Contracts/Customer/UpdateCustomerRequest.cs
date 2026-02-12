using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Customer;

public class UpdateCustomerRequest
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int AddressId { get; set; }
}
