using Alsalamony.Results;
using Application.Services.InvoiceItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alsalamony2.Controllers;

[Route("InvoiceItem")]
[ApiController]
public class InvoiceItemController : ControllerBase
{
    private readonly IInvoiceItemSerivces invoiceItemSerivces;

    public InvoiceItemController(IInvoiceItemSerivces invoiceItemSerivces)
    {
        this.invoiceItemSerivces = invoiceItemSerivces;
    }

    [HttpGet("GetInvoiceItemsByInvoiceId/{invoiceId}")]
    public IActionResult GetInvoiceItemsByInvoiceId([FromRoute]int invoiceId)
    {
        var re = invoiceItemSerivces.GetInvoiceItemsByInvoiceId(invoiceId);
        return re.IsSuccess? Ok(re.Value):re.ToProblem();
    }
}
