using Application.Common.Results;
using Microsoft.AspNetCore.Http;


namespace Application.Common.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFound =
        new("User.NotFound", "No User was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error UserAddFailed =
        new("User.AddFailed", "Failed to add the new User", StatusCodes.Status500InternalServerError);
}
