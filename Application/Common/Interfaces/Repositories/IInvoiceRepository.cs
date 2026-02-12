using Application.Contracts.Invoice;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    ICollection<Invoice> GetInvoicesByCustomerId(int customerId);

    List<UnpaidInvoiceRow> GetAllUnpayedRowsByCustomerId(int customerId);
}
