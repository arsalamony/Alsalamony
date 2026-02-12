using Application.Common.Results;
using Application.Contracts.Customer;


namespace Application.Services.Customer;

public interface ICustomerServices
{
    Result<IEnumerable<CustomerViewResponse>> GetAllCustomers();

    Result<CustomerResponse> Get(int id);

    Result<CustomerResponse> Add(AddCustomerRequest request);

    Result Update(int Id, UpdateCustomerRequest request);

    Result Delete(int Id);
}
