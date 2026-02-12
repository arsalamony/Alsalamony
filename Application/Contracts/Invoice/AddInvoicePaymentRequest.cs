using Domain.Enums;

namespace Application.Contracts.Invoice;

public class AddInvoicePaymentRequest
{
    public int InvoiceId { get; set; }
    public decimal AmountPaid { get; set; }
    public string? Notes { get; set; }
    public enPaymentMethod PaymentMethod { get; set; }
}
