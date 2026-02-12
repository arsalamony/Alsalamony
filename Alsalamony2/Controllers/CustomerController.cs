
using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Application.Contracts.Customer;
using Application.Services.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony.Controllers;

[EnableRateLimiting(RateLimiters.UserLimiter)]
[Route("Customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    public ICustomerServices Customer { get; }

    public CustomerController(ICustomerServices customer)
    {
        Customer = customer;
    }


    [Authorize]
    [HttpGet("All")]
    public IActionResult GetAllCustomers()
    {

        return Ok(Customer.GetAllCustomers().Value);
    }


    [Authorize]
    [HttpGet("Get/{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var result = Customer.Get(id);
        return result.IsSuccess? Ok(result.Value):result.ToProblem();
    }

    [Authorize]
    [HttpPost("Add")]
    public IActionResult Add(AddCustomerRequest request)
    {
        var result = Customer.Add(request);
        return result.IsSuccess? CreatedAtAction(nameof(Get), new {id = result.Value.Id}, result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpPut("Update/{id}")]
    public IActionResult Update([FromRoute] int id, UpdateCustomerRequest request)
    {
        
        var result = Customer.Update(id, request);

        return result.IsSuccess? NoContent():result.ToProblem();
    }

    [Authorize(Roles ="Admin")]
    [HttpDelete("Delete")]
    public IActionResult Delete([FromQuery] int id)
    {
        var result = Customer.Delete(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
