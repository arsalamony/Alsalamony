using Application.Common.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Errors;

public class UserProductErrors
{
    public static readonly Error UserProductNotFound =
    new("UserProduct.NotFound", "No UserProduct was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error UserProductAddFailed =
        new("UserProduct.AddFailed", "Failed to add the new UserProduct", StatusCodes.Status500InternalServerError);
}
