using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Payment;

public class PaymentViewResponse
{
    public int PaymentId { get; set; }
    public int? InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;
    public bool Added { get; set; }

    public bool Finshed { get; set; }
    public string? Notes { get; set; }
}
