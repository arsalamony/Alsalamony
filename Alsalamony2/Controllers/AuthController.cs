using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Application.Contracts.Auth;
using Application.Contracts.User;
using Application.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;


namespace Alsalamony.Controllers;

[Route("Auth")]
[EnableRateLimiting(RateLimiters.IpLimiter)]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthServices authServices;

    public AuthController(IAuthServices authServices)
    {
        this.authServices = authServices;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        
        var result = authServices.Login(request);
        return result.IsSuccess? Ok(result.Value) : result.ToProblem();
    }

    [Authorize(Roles ="Admin")]
    [HttpPost("Register")]
    public IActionResult Register([FromBody] AddUserRequest request)
    {
        var re = authServices.Register(request);
        return re.IsSuccess? NoContent():re.ToProblem();
    }
}
