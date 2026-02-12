using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Application.Services.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony2.Controllers;

[EnableRateLimiting(RateLimiters.UserLimiter)]
[Authorize]
[Route("Address")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressServices addressServices;

    public AddressController(IAddressServices addressServices)
    {
        this.addressServices = addressServices;
    }

    [HttpGet("All")]

    public IActionResult GetAll() 
    {
        var result = this.addressServices.GetAll();

        return result.IsSuccess? Ok(result.Value) : result.ToProblem();
    }

}
