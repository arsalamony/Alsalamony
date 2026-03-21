using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface IInvoiceItemRepository : IGenericRepository<InvoiceItem>
{
    Task<ICollection<InvoiceItem>> GetInvoiceItems(int InvoiceId);
}
