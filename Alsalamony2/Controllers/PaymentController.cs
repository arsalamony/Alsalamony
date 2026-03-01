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
    public IActionResult GetPayment(int paymentId)
    {
        var result = paymentServices.Get(paymentId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("Add")]
    public IActionResult AddPayment([FromBody] AddPaymentRequest request) 
    {
        var result = paymentServices.Add(User.GetUserId(), request);
        return result.IsSuccess ? CreatedAtAction(nameof(GetPayment), new { paymentId = result.Value.PaymentId }, result.Value) : result.ToProblem();
    }

    [Authorize(Roles ="Admin")]
    [HttpPost("AddByAdmin")]
    public IActionResult AddPaymentByAdmin([FromBody] AddPaymentByAdminRequest request)
    {
        var result = paymentServices.AddByAdmin(request);
        return result.IsSuccess? CreatedAtAction(nameof(GetPayment), new { paymentId = result.Value.PaymentId }, result.Value) : result.ToProblem();
    }


    [Authorize(Roles ="Admin")]
    [HttpGet("GetAll")]
    public IActionResult GetAllPayments([FromQuery] int PageNo,[FromQuery] int PageSize)
    {
        var result = paymentServices.GetAllPaged(PageNo, PageSize);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("PaymentNo")]
    public IActionResult GetPaymentsNo()
    {
        var result = paymentServices.GetPaymentsNo();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpGet("GetAll/{userId}")]
    public IActionResult GetAllPayments([FromRoute]int userId)
    {
        var result = paymentServices.GetAll(userId, User.IsAdmin());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize(Roles ="Admin")]
    [HttpDelete("Delete/{paymentId}")]
    public IActionResult DeletePayment([FromRoute] int paymentId)
    {
        var result = paymentServices.Delete(paymentId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [Authorize(Roles ="Admin")]
    [HttpPost("FinshAllPayment/{userId}")]
    public IActionResult FinshAllPayment([FromRoute] int userId)
    {
        var result = paymentServices.FinshAllPayment(userId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("FinshPayment/{paymentId}")]
    public IActionResult FinshPayment([FromRoute] int paymentId)
    {
        var result = paymentServices.FinshPayment(paymentId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
