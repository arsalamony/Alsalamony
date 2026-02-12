using Application.Common.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors;

public class PaymentErrors
{
    public static readonly Error PaymentNotFound =
        new("Payment.NotFound", "No Payment was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error PaymentAddFailed =
    new("Payment.AddFailed", "Failed to add the new Payment", StatusCodes.Status500InternalServerError);

    public static readonly Error PaymentUpdateFailed =
    new("Payment.UpdateFailed", "Failed to Update the Payment", StatusCodes.Status500InternalServerError);
}
