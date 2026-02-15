using Application.Common.Results;
using Application.Contracts.Invoice;


namespace Application.Services.Invoice;

public interface IInvoiceServices
{
    Result<IEnumerable<InvoiceResponse>> GetAllUnpayed(int customerId);
    Result<InvoiceResponse> Get(int invoiceId);

    Result<InvoiceResponse> Add(int UserId, AddInvoiceRequest request);

    Result InvoicePayment(int UserId, AddInvoicePaymentRequest request);

    Result FullDelete(int InvoiceId);
}
