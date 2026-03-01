using Application.Common.Results;
using Application.Contracts.Payment;


namespace Application.Services.Payment;

public interface IPaymentServices
{
    Result<PaymentResponse> Get(int Id);

    Result<PaymentResponse> Add(int UserId, AddPaymentRequest request);

    Result<PaymentResponse> AddByAdmin(AddPaymentByAdminRequest request);

    Result<IEnumerable<PaymentViewResponse>> GetAllPaged(int PageNo, int RowsNo);

    Result<IEnumerable<PaymentViewResponse>> GetAll(int UserId, bool IsAdmin);

    Result<int> GetPaymentsNo();

    Result Delete(int PaymentId);

    Result FinshAllPayment(int UserId);

    Result FinshPayment(int PaymentId);
}
