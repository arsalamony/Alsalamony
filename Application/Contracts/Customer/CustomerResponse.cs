using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Customer;

public class CustomerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int AddressId { get; set; }
}
