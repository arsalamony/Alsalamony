using Application.Common.Results;
using Application.Contracts.InvoiceItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InvoiceItem;

public interface IInvoiceItemSerivces
{
    Result<IEnumerable<InvoiceItemResponse>> GetInvoiceItemsByInvoiceId(int invoiceId); 
}
