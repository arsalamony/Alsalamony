using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.InvoiceItem;

public class AddInvoiceItemRequest
{
    public int ProductId { get; set; }

    public int Qty { get; set; }

    public decimal PricePerUnit { get; set; }

    public bool IsGift { get; set; }
}
