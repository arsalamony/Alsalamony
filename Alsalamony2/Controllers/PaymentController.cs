using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Alsalamony.Utilities;
using Application.Contracts.Payment;
using Application.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony.Controllers;

[EnableRateLimiting(RateLimiters.UserLimiter)]
[Route("Payment")]
[Authorize]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentServices paymentServices;

    public PaymentController(IPaymentServices paymentServices)
    {
        this.paymentServices = paymentServices;
    }


    [Authorize]
    [HttpGet("Get/{paymentId}")]
    public async Task<IActionResult> GetPayment(int paymentId)
    {
        var result = await paymentServices.Get(paymentId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("Add")]
    public async Task<IActionResult> AddPayment([FromBody] AddPaymentRequest request) 
    {
        var result = await paymentServices.Add(User.GetUserId(), request);
        return result.IsSuccess ? CreatedAtAction(nameof(GetPayment), new { paymentId = result.Value.PaymentId }, result.Value) : result.ToProblem();
    }

    [Authorize(Roles ="Admin")]
    [HttpPost("AddByAdmin")]
    public async Task<IActionResult> AddPaymentByAdmin([FromBody] AddPaymentByAdminRequest request)
    {
        var result = await paymentServices.AddByAdmin(request);
        return result.IsSuccess? CreatedAtAction(nameof(GetPayment), new { paymentId = result.Value.PaymentId }, result.Value) : result.ToProblem();
    }


    [Authorize(Roles ="Admin")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllPayments([FromQuery] int PageNo,[FromQuery] int PageSize)
    {
        var result = await paymentServices.GetAllPaged(PageNo, PageSize);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("PaymentNo")]
    public async Task<IActionResult> GetPaymentsNo()
    {
        var result = await paymentServices.GetPaymentsNo();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpGet("GetAll/{userId}")]
    public async Task<IActionResult> GetAllPayments([FromRoute]int userId)
    {
        var result = await paymentServices.GetAll(userId, User.IsAdmin());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize(Roles ="Admin")]
    [HttpDelete("Delete/{paymentId}")]
    public async Task<IActionResult> DeletePayment([FromRoute] int paymentId)
    {
        var result = await paymentServices.Delete(paymentId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [Authorize(Roles ="Admin")]
    [HttpPost("FinshAllPayment/{userId}")]
    public async Task<IActionResult> FinshAllPayment([FromRoute] int userId)
    {
        var result = await paymentServices.FinshAllPayment(userId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("FinshPayment/{paymentId}")]
    public async Task<IActionResult> FinshPayment([FromRoute] int paymentId)
    {
        var result = await paymentServices.FinshPayment(paymentId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
