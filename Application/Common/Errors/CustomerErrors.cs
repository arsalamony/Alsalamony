using Application.Common.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Errors;

public static class CustomerErrors
{
    public static readonly Error CustomerNotFound =
        new("Customer.NotFound", "No Customer was found with the given ID", StatusCodes.Status404NotFound);


    public static readonly Error Forbidden =
        new("Customer.Forbidden", "You are not allowed to access this Customer.", StatusCodes.Status403Forbidden);
}
