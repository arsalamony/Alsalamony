using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Product;

public class AddProductRequest
{
    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }
}
