

using Application.Common.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Errors;

public static class InvoiceErrors
{
    public static readonly Error InvoiceNotFound =
        new("Invoice.NotFound", "No Invoice was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error InvoiceAddFailed =
        new("Invoice.AddFailed", "Failed to add the new Invoice", StatusCodes.Status500InternalServerError);

    public static readonly Error InvoicePaymentAddFailed =
    new("InvoicePayment.AddFailed", "Failed to add the new Invoice Payment", StatusCodes.Status500InternalServerError);
}
