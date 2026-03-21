using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Alsalamony.Utilities;
using Application.Contracts.UserProduct;
using Application.Services.UserProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony.Controllers;

[Route("UserProduct")]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[ApiController]
public class UserProductController : ControllerBase
{
    private readonly IUserProductServices userProductServices;

    public UserProductController(IUserProductServices userProductServices)
    {
        this.userProductServices = userProductServices;
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateUserProductQuantity")]
    public async Task<IActionResult> UpdateUserProductQuantity(UpdateUserProductQuantityRequest request)
    {
        var result = await userProductServices.UpdateUserProductQuantity(request);
        return result.IsSuccess? NoContent():result.ToProblem();
    }

    [Authorize]
    [HttpPut("TransUserProductQuantity")]
    public async Task<IActionResult> IncDecUserProductQuantity(UpdateUserProductQuantityRequest request)
    {
        int UserId = User.GetUserId();
        var result = await userProductServices.TransUserProductQuantity(UserId ,request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
