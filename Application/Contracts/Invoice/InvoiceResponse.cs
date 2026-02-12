

using Application.Contracts.InvoiceItem;

namespace Application.Contracts.Invoice;

public class InvoiceResponse
{

    public int InvoiceId { get; set; }

    public string? CustomerName { get; set; }

    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal RemainingAmount { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Notes { get; set; }

    public IEnumerable<InvoiceItemResponse> InvoiceItems { get; set; } = Enumerable.Empty<InvoiceItemResponse>();
}
