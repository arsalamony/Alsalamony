namespace Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


public class UserProduct
{
    public int UserProductId { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Qty { get; set; }

    public Product? Product { get; set; }
}
