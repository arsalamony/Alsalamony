using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Application.Contracts.Product;
using Application.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony2.Controllers;

[Route("Product")]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductServices productServices;

    public ProductController(IProductServices productServices)
    {
        this.productServices = productServices;
    }

    [Authorize]
    [HttpGet("All")]
    public IActionResult GetAll() 
    {
        var re = productServices.GetAll();
        return re.IsSuccess? Ok(re.Value) : re.ToProblem();
    }

    [Authorize]
    [HttpPost("Add")]
    public IActionResult Add(AddProductRequest request) 
    {
        var re = productServices.Add(request);
        return re.IsSuccess? NoContent():re.ToProblem();
    }
}
