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
    public async Task<IActionResult> GetAllUnpayed([FromQuery] int CustomerId) 
    {
        var result = await invoiceServices.GetAllUnpayed(CustomerId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpGet("Get/{invoiceId}")]
    public async Task<IActionResult> GetInvoice([FromRoute]int invoiceId)
    {
        var result = await invoiceServices.Get(invoiceId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("Add")]
    public async Task<IActionResult> AddInvoice([FromBody] AddInvoiceRequest request)
    {
        int UserId = User.GetUserId();
        var result = await invoiceServices.Add(UserId, request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPost("InvoicePayment")]
    public async Task<IActionResult> InvoicePayment([FromBody] AddInvoicePaymentRequest request)
    {
        int UserId = User.GetUserId();
        var result = await invoiceServices.InvoicePayment(UserId, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("FullDelete/{id}")]
    public async Task<IActionResult> FullDelete(int id) 
    {
        var re = await invoiceServices.FullDelete(id);
        return re.IsSuccess? NoContent() : re.ToProblem();
    }


}
