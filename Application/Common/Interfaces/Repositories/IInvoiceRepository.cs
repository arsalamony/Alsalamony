using Application.Contracts.Invoice;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<ICollection<Invoice>> GetInvoicesByCustomerId(int customerId);

    Task<List<UnpaidInvoiceRow>> GetAllUnpayedRowsByCustomerId(int customerId);
}
