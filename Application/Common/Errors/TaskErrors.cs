using Application.Common.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Errors;

public class TaskErrors
{
    public static readonly Error TaskNotFound =
    new("Task.NotFound", "No Task was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error TaskAddFailed =
        new("Task.AddFailed", "Failed to add the new Task", StatusCodes.Status500InternalServerError);
}
