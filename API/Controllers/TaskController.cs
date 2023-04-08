using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("task")]
public class TaskController : ControllerBase
{
    private readonly ITaskManager _taskManager;

    public TaskController(ITaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get() // какое ты собрался получать?

    {
        var task = await _taskManager.GetAny();
        if (task is null)
            return StatusCode(204);
        return Ok(task);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("add/{groupId:long}")]
    public async Task<IActionResult> Add([FromBody] TaskApiModel model, long groupId)
    {
        // _taskManager.GetCurrentForStudent()
        throw new NotImplementedException();
    }
}