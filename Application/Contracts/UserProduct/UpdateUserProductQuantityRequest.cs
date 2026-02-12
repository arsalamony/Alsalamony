using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.UserProduct;

public class UpdateUserProductQuantityRequest
{
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Qty { get; set; }
}
