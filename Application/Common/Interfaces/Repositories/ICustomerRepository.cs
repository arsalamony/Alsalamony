using Application.Contracts.Customer;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    IEnumerable<CustomerViewResponse> GetAllCustomers();
}
