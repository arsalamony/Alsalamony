using Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Alsalamony.Results;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var problem = Microsoft.AspNetCore.Http.Results.Problem(statusCode: result.Error.StatusCode);

        var problemDetails = problem.GetType()
            .GetProperty(nameof(ProblemDetails))!
            .GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors",
                new [] {
                    new
                    {
                        result.Error.Code,
                        result.Error.Description
                    }
                }
            }
        };

        return new ObjectResult(problemDetails);
    }
}
