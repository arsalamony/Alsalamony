using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Customer;

public class CustomerViewResponse
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Dept { get; set; } = 0;
}

