using Alsalamony.Domain.Consts;
using Application.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony2.Controllers;

[Route("Report")]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[Authorize]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportServices reportServices;

    public ReportController(IReportServices reportServices)
    {
        this.reportServices = reportServices;
    }

    [HttpGet("Income")]
    [Authorize(Roles ="Admin")]
    public IActionResult GetIncomeReport()
    {
        var result = reportServices.GetDayIncomeReport();
        return Ok(result.Value);
    }

}
