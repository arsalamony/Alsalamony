using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.InvoiceItem;

public class InvoiceItemResponse
{
    public string ProductName { get; set; } = null!;

    public int Qty { get; set; }

    public decimal PricePerUnit { get; set; }

    public bool IsGift { get; set; }
}
