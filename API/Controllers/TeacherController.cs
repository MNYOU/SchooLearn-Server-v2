using System.Net.Mail;
using System.Security.Claims;
using Dal.Enums;
using Dal.Repositories;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = nameof(Role.Teacher))]
[Route("teacher")]
public class TeacherController: ControllerBase
{
    private readonly ITeacherManager _manager;
    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");

    public TeacherController(ITeacherManager manager)
    {
        _manager = manager;
    }

    [HttpGet("groups")]
    public IActionResult GetMyGroups([FromQuery] long? subjectId)
    {
        var groups = _manager.GetMyGroupsApiModels(Id, subjectId);
        return Ok(groups);
    }
    
    [HttpPost("group/create")]
    public async Task<IActionResult> Create([FromBody] GroupApiModel model, [FromQuery] long subjectId)
    {
        var result = await _manager.CreateGroup(Id, subjectId,  model);
        return result ? Ok() : StatusCode(500);
    }

    [HttpDelete("group/{groupId:long}/disband")]
    public async Task<IActionResult> DisbandGroup([FromRoute] long groupId)
    {
        var result = await _manager.DisbandGroup(Id, groupId);
        return result ? Ok() : StatusCode(500);
    }

    [HttpGet("group/{groupId:long}/all")]
    public IActionResult GetStudentsFromGroup([FromRoute] long groupId)
    {
        var students = _manager.GetStudentsFromGroup(Id, groupId);
        return Ok(students);
    }

    [HttpGet("group/{groupId:long}/applications")]
    public IActionResult GetApplicationsToGroup([FromRoute] long groupId)
    {
        var students = _manager.GetStudentApplications(Id, groupId);
        return Ok(students);
    }

    [HttpPut("group/{groupId:long}/application/consider")]
    public IActionResult ConsiderApplication([FromRoute] long groupId, [FromQuery] long studentId, [FromQuery] bool isApproved)
    {
        var result = _manager.ConsiderApplication(Id, groupId, studentId, isApproved);
        return result ? Ok() : StatusCode(500);
    }

    [HttpGet("group/{groupId:long}/code")]
    public async Task<IActionResult> GetGroupInvitationCodeAsync([FromRoute] long groupId)
    {
        var code = await _manager.GetGroupInvitationCodeAsync(Id, groupId);
        return code != null
            ? Ok(new {Code = code})
            : BadRequest();
    }

    [HttpPut("group/{groupId:long}/generate-code")]
    public IActionResult GenerateNewInvitationCode([FromRoute] long groupId)
    {
        var result = _manager.GenerateNewInvitationCode(Id, groupId);
        return result ? Ok(true) : Ok(false);
    }


    [HttpGet("subjects")]
    public IActionResult GetMySubjects([FromServices] ITaskManager taskManager)
    {
        var subjects = taskManager.GetSubjects();
        return Ok(subjects);
    }

    [HttpPost("subject/add")]
    [Obsolete]
    public async Task<IActionResult> AddSubjectToMy([FromBody] SubjectApiModel model)
    {
        return StatusCode(501);
        var result = await _manager.AddSubject(Id, model);
        return result ? Ok() : StatusCode(500);
    }
}