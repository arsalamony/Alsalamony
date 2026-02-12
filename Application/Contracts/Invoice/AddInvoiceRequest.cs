using Application.Contracts.InvoiceItem;
using Domain.Enums;


namespace Application.Contracts.Invoice;

public class AddInvoiceRequest
{
    public int? CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal AmountPaid { get; set; }

    public enPaymentMethod PaymentMethod { get; set; }
    public string? Notes { get; set; }

    public IEnumerable<AddInvoiceItemRequest> InvoiceItems { get; set; } = new List<AddInvoiceItemRequest>();
}
