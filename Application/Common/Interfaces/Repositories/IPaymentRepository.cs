using Application.Contracts.Payment;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<IEnumerable<Payment>> GetAll();
    Task<IEnumerable<PaymentViewResponse>> GetAllPaged(int PageNo, int RowsNo);
    Task<IEnumerable<Payment>> GetAll(int InvoiceId);

    Task<int> GetPaymentNo();
}
