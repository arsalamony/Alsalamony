using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Product;

public class ProductViewResponse
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

}
