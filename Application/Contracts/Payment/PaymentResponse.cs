using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Payment;

public class PaymentResponse
{
    public int PaymentId { get; set; }

    public int? InvoiceId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Finshed { get; set; } = null!;

    public string? Notes { get; set; }
}
