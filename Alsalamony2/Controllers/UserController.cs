using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Alsalamony.Utilities;
using Application.Contracts.User;
using Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony.Controllers;

[Route("User")]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserServices _User;
    public UserController(IUserServices user)
    {
        _User = user;
    }

    [Authorize]
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        var re = _User.GetUsers();
        return re.IsSuccess ? Ok(re.Value) : re.ToProblem();
    }

    [Authorize]
    [HttpGet("Get/{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var result = _User.Get(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("Delete/{id}")]
    public IActionResult Delete([FromRoute] int id) 
    {
        var re = _User.Delete(id);
        return re.IsSuccess? NoContent() : re.ToProblem();
    }

    [Authorize]
    [HttpPut("UpdateLocation")]
    public IActionResult UpdateLocation(GetLocationRequest request) 
    {
        var re = _User.GetLocation(User.GetUserId(), request);
        return re.IsSuccess? NoContent(): re.ToProblem();
    }
}
