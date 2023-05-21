using Dal.Enums;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = nameof(Role.Student))]
[Route("student/")]
public class StudentController : ControllerBase
{
    private readonly IStudentManager _manager;

    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");

    public StudentController(IStudentManager manager)
    {
        _manager = manager;
    }

    [HttpGet("groups")]
    public IActionResult GetMyGroups()
    {
        var groups = _manager.GetMyGroups(Id);
        return Ok(groups);
    }

    [HttpGet("groups/{groupId:long}")]
    public IActionResult GetTeacherOfGroup([FromRoute] long groupId)
    {
        var teacherName = _manager.GetTeacherName(Id, groupId);
        return teacherName != null 
            ? Ok(teacherName) 
            : BadRequest();
    }


    [HttpPost("group/{groupId:long}/create-application")]
    public async Task<IActionResult> CreateApplicationToGroupAsync([FromRoute] long groupId,
        [FromQuery] string invitationCode, [FromServices] ITeacherManager teacherManager)
    {
        var result = await _manager.CreateApplicationToGroup(Id, groupId, invitationCode, teacherManager);
        return result ? Ok() : BadRequest();
    }

    [HttpDelete("group/{groupId:long}/leave")]
    public async Task<IActionResult> LeaveGroupAsync([FromRoute] long groupId)
    {
        var result = await _manager.LeaveGroupAsync(Id, groupId);
        return result ? Ok() : StatusCode(500);
    }
}