using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.UserProduct;

public class UserProductResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }
}
