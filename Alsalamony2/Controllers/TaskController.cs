using Alsalamony.Domain.Consts;
using Alsalamony.Results;
using Alsalamony.Utilities;
using Application.Common.Results;
using Application.Contracts.Task;
using Application.Services.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Alsalamony2.Controllers;

[Route("Task")]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskServices taskServices;

    public TaskController(ITaskServices taskServices)
    {
        this.taskServices = taskServices;
    }

    [HttpGet("All")]
    [Authorize]
    public async Task<IActionResult> GetAll() 
    {
        var re = await taskServices.GetAll();
        return re.IsSuccess ? Ok(re.Value) : re.ToProblem();
    }

    [HttpGet("Get/{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id) 
    {
        var re = await taskServices.Get(id);
        return re.IsSuccess? Ok(re.Value) : re.ToProblem();
    }

    [HttpPost("Add")]
    [Authorize]
    public async Task<IActionResult> Add([FromBody]AddTaskRequest request) 
    {
        var re = await taskServices.Add(User.GetUserId() ,request);
        return re.IsSuccess ? CreatedAtAction(nameof(Get), new { id = re.Value.TaskId }, re.Value) : re.ToProblem();
    }

    [HttpPut("SetComplete/{id}")]
    [Authorize]
    public async Task<IActionResult> SetComplete([FromRoute] int id)
    {
        var re = await taskServices.SetComplete(id, User.GetUserId());
        return re.IsSuccess ? NoContent() : re.ToProblem();
    }

    [HttpPut("SetCancel/{id}")]
    [Authorize]
    public async Task<IActionResult> SetCancel([FromRoute] int id, [FromBody]string Notes)
    {
        var re = await taskServices.SetCancel(id, User.GetUserId(), Notes);
        return re.IsSuccess ? NoContent() : re.ToProblem();
    }

    [HttpDelete("Delete/{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Delete([FromRoute]int id) 
    {
        var re = await taskServices.Delete(id);
        return re.IsSuccess? NoContent(): re.ToProblem();
    }
}
