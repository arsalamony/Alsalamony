using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Alsalamony.Utilities;
using Application.Contracts.Invoice;
using Application.Services.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony.Controllers;

[EnableRateLimiting(RateLimiters.UserLimiter)]
[Route("Invoice")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceServices invoiceServices;

    public InvoiceController(IInvoiceServices invoiceServices)
    {
        this.invoiceServices = invoiceServices;
    }

    [Authorize]
    [HttpGet("GetAllUnpayed")]
    public IActionResult GetAllUnpayed([FromQuery] int CustomerId) 
    {
        var result = invoiceServices.GetAllUnpayed(CustomerId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpGet("Get/{invoiceId}")]
    public IActionResult GetInvoice([FromRoute]int invoiceId)
    {
        var result = invoiceServices.Get(invoiceId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("Add")]
    public IActionResult AddInvoice([FromBody] AddInvoiceRequest request)
    {
        int UserId = User.GetUserId();
        var result = invoiceServices.Add(UserId, request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("InvoicePayment")]
    public IActionResult InvoicePayment([FromBody] AddInvoicePaymentRequest request)
    {
        int UserId = User.GetUserId();
        var result = invoiceServices.InvoicePayment(UserId, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
