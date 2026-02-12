using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Address;

public class AddressResponse
{
    public int AddressId { get; set; }

    public string AddressName { get; set; } = null!;
}
