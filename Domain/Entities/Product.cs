namespace Domain.Entities;

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


public class Product
{

    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }

    // Navigation
    //public ICollection<InvoiceItem> InvoiceItems { get; set; }
    //public ICollection<EmployeeDailyStock> EmployeeDailyStocks { get; set; }
    //public ICollection<EmployeeCardAction> EmployeeCardActions { get; set; }

}
