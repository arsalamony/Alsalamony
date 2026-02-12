namespace Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


public class Invoice
{
    public int InvoiceId { get; set; }

    public int? CustomerId { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    public decimal TotalAmount { get; set; } = 0;

    public decimal AmountPaid { get; set; } = 0;

    public decimal RemainingAmount { get; set; } = 0;

    public int CreatedByUserId { get; set; }

    public string? Notes { get; set; }


    public User? CreatedByUser { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
