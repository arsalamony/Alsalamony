namespace Domain.Entities;

public class InvoiceItem
{
    public int InvoiceItemId { get; set; }

    public int InvoiceId { get; set; }

    public int ProductId { get; set; }

    public int Qty { get; set; }

    public decimal PricePerUnit { get; set; }

    public bool IsGift { get; set; } = false;

    public Product? Product { get; set; }

    public Invoice? Invoice { get; set; }
}
