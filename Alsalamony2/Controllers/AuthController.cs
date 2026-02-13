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
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthServices authServices, ILogger<AuthController> logger)
    {
        this.authServices = authServices;
        this._logger = logger;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Logging with email: {Username}", request.Username);

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
