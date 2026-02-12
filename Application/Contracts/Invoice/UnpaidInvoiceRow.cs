using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Invoice;

public sealed class UnpaidInvoiceRow
{
    public int InvoiceId { get; set; }
    public string CustomerName { get; set; } = null!;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingAmount { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string? Notes { get; set; }



    public string? ProductName { get; set; }
    public int? Qty { get; set; }
    public decimal? PricePerUnit { get; set; }
    public bool? IsGift { get; set; }
}