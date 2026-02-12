using Application.Common.Results;
using Microsoft.AspNetCore.Http;


namespace Application.Common.Errors;

public class SystemRecordErrors
{
    public static readonly Error SystemRecordNotFound =
    new("SystemRecord.NotFound", "No SystemRecord was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error SystemRecordUpdateFailed =
        new("SystemRecord.UpdateFailed", "Failed to Update the new SystemRecord", StatusCodes.Status500InternalServerError);
}
