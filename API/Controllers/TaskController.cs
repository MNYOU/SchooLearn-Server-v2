using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Dal.Entities;
using Dal.Enums;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("task/")]
public class TaskController : ControllerBase
{
    private readonly ITaskManager _manager;

    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");

    private Role UserRole
    {
        get
        {
            var value = User.FindFirst(ClaimTypes.Role)?.Value;
            return value != null ? Enum.Parse<Role>(value) : Role.Default;
        }
    }

    public TaskController(ITaskManager manager)
    {
        _manager = manager;
    }

    [Authorize(Roles = $"{nameof(Role.Teacher)},{nameof(Role.Student)}")]
    [HttpGet("{taskId:long}/")]
    public IActionResult Get([FromRoute] long taskId)
    {
        var task = UserRole switch
        {
            Role.Student => _manager.GetForStudent(Id, taskId),
            Role.Teacher => _manager.GetForTeacher(Id, taskId),
            _ => null
        };
        if (task is null)
            return StatusCode(500);
        return Ok(task);
    }

    [Authorize]
    [HttpGet("any")]
    public IActionResult GetAny([FromQuery] long? subjectId,
        [FromQuery] long? difficultyId, [FromServices] IAccountManager accountManager)
    {
        var task = _manager.GetAny(Id, subjectId, difficultyId, accountManager);
        if (task is null)
            return BadRequest();
        // return StatusCode(204);
        return Ok(task);
    }

    [HttpGet("subjects/")]
    public IActionResult GetSubjects()
    {
        var subjects = _manager.GetSubjects();
        return Ok(subjects);
    }

    [HttpGet("difficulties/")]
    public IActionResult GetDifficulties()
    {
        return Ok(_manager.GetAvailableDifficulties());
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpGet("current/")]
    public async Task<IActionResult> GetCurrentForStudentAsync([FromQuery] long groupId)
    {
        var tasks = await _manager.GetCurrentForStudentAsync(Id, groupId);
        return Ok(tasks);
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpGet("expired/all")]
    public async Task<IActionResult> GetExpiredForStudentAsync([FromQuery] DateTime? period)
    {
        var tasks = await _manager.GetExpiredTasksAsync(Id, period);
        return Ok(tasks);
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpGet("solved/")]
    public IActionResult GetSolvedTasks([FromQuery] long groupId)
    {
        var tasks = _manager.GetSolvedTasksPreviewAsync(Id, groupId);
        return Ok(tasks);
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpGet("solved/{taskId:long}")]
    public IActionResult GetSolvedTask([FromRoute] long taskId)
    {
        var task = _manager.GetSolvedTask(Id, taskId);
        if (task is null)
            return BadRequest();
        return Ok(task);
    }


    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpGet("assigned/")]
    public async Task<IActionResult> GetAssignedAsync([FromQuery] long? groupId, [FromQuery] DateTime? period)
    {
        var tasks = await _manager.GetAssignedTasksAsync(Id, groupId, period);
        return Ok(tasks);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpGet("outdated/")]
    public async Task<IActionResult> GetOutdatedAsync([FromQuery] DateTime? period)
    {
        var tasks = await _manager.GetOutdatedTasksAsync(Id, period);
        return Ok(tasks);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpGet("/{taskId:long}/completed-students")]
    public IActionResult GetStudentsCompetedTasks([FromRoute] long taskId)
    {
        var students = _manager.GetStudentsWhoCompletedTask(Id, taskId);
        return Ok(students);
    }


    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] TaskApiModel model, [FromQuery] long? groupId)
    {
        if (model is { IsExtended: false, Answer: null or "" })
            return BadRequest("Ответ не может быть null, если задание не является расширенным");
        if (model.Deadline <= DateTime.Now)
            return BadRequest();
        var result = false;
        if (groupId is null)
            result = await _manager.TryAddTaskInRepositoryAsync(Id, model);
        else
            result = await _manager.TryAddTaskForGroupAsync(Id, groupId.Value, model);
        return result ? Ok() : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("/{taskId:long}/for-groups")]
    public async Task<IActionResult> AddForGroups([FromBody] IEnumerable<GroupApiModel> groups, [FromRoute] long taskId)
    {
        var result = await _manager.TryAddTaskForGroupsAsync(Id, taskId, groups);
        return result
            ? Ok()
            : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("/update")]
    public async Task<IActionResult> UpdateAsync([FromBody] TaskApiModel model)
    {
        var result = await _manager.UpdateTaskAsync(Id, model);
        return result
            ? Ok()
            : BadRequest();
    }


    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("/{taskId:long}/delete")]
    public IActionResult Delete([FromRoute] long taskId)
    {
        var result = _manager.DeleteTask(Id, taskId);
        return result
            ? Ok()
            : BadRequest();
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpPost("check/")]
    public IActionResult Check([FromBody] TaskWithAnswerRequest model)
    {
        var result = _manager.CheckAndPointTask(model, Id);
        return result ? Ok() : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpPost("/{taskId:long}/upload-answer/")]
    public async Task<IActionResult> UploadAnswer([FromBody] IFormFile file, [FromRoute] long taskId)
    {
        if (file.ContentType != "application/pdf")
            return BadRequest("тип фийла должен быть .pdf");
        var result = await _manager.UploadAnswerToTaskAsync(Id, taskId, file);
        return result
            ? Ok()
            : StatusCode(500);
    }

    [Authorize(Roles = $"{nameof(Role.Student)},{nameof(Role.Teacher)}")]
    [HttpGet("/{taskId:long}/download-answer")]
    public IActionResult DownloadAnswer([FromRoute] long taskId, [FromQuery] long? studentId)
    {
        var fileAnswer = new FileAnswer();
        switch (UserRole)
        {
            case Role.Student:
                fileAnswer = _manager.DownloadAnswer(Id, taskId);
                break;
            case Role.Teacher when studentId != null:
                fileAnswer = _manager.DownloadAnswer(studentId.Value, taskId);
                break;
            default:
                return BadRequest();
        }
        return File(fileAnswer.Content, fileAnswer.ContentType, fileAnswer.FileName);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpGet("unchecked/")]
    public IActionResult GetUncheckedTasks([FromQuery] long? groupId)
    {
        var tasks = _manager.GetUnchecked(Id, groupId);
        return Ok(tasks);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("rate/")]
    public async Task<IActionResult> RateAnswerAsync([FromQuery, Range(0, 10)] byte scores, [FromQuery] long studentId,
        [FromQuery] long taskId)
    {
        var result = await _manager.RateAnswerAsync(Id, studentId, taskId, scores);
        return result ? Ok() : StatusCode(500);
    }
}