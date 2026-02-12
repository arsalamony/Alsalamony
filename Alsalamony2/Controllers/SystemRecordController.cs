using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Alsalamony.Utilities;
using Application.Services.SystemRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony2.Controllers;


[Route("SystemRecord")]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[ApiController]
public class SystemRecordController : ControllerBase
{
    private readonly ISystemRecordServices systemRecordServices;

    public SystemRecordController(ISystemRecordServices systemRecordServices)
    {
        this.systemRecordServices = systemRecordServices;
    }

    [HttpGet("All")]
    [Authorize]
    public IActionResult GetAll() 
    {
        var re = systemRecordServices.GetAllNotFinished(User.IsAdmin());
        return re.IsSuccess? Ok(re.Value) : re.ToProblem();
    }

    [HttpPut("Finish/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Finish([FromRoute] int id) 
    {
        var re = systemRecordServices.Finish(id);
        return re.IsSuccess ? NoContent() : re.ToProblem();
    }
}
