using System.ComponentModel.DataAnnotations;
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

    public TaskController(ITaskManager manager)
    {
        _manager = manager;
    }

    [HttpGet("/any")]
    public IActionResult GetAny([FromQuery] long institutionId, [FromQuery] string? subject, [FromQuery] string? difficulty)
    {
        var task = _manager.GetAny(institutionId, subject, difficulty);
        if (task is null)
            return BadRequest();
            // return StatusCode(204);
        return Ok(task);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("current/")]
    public async Task<IActionResult> GetCurrentForStudentAsync()
    {
        var tasks = await _manager.GetCurrentForStudentAsync(Id);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("expired/all")]
    public async Task<IActionResult> GetExpiredForStudentAsync([FromQuery] DateTime? period)
    {
        var tasks = await _manager.GetExpiredTasksAsync(Id, period);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("solved/all")]
    public IActionResult GetSolvedTasks()
    {
        var tasks =  _manager.GetSolvedTasksPreviewAsync(Id);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("solved/{taskId:long}")]
    public IActionResult GetSolvedTask([FromRoute] long taskId)
    {
        var task = _manager.GetSolvedTask(Id, taskId);
        if (task != null)
            return BadRequest();
        return Ok(task);
    }
    

    [Authorize(nameof(Role.Teacher))]
    [HttpGet("assigned/")]
    public async Task<IActionResult> GetAssignedAsync([FromQuery] DateTime? period)
    {
        var tasks = await _manager.GetAssignedTasksAsync(Id, period);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Teacher))]
    [HttpGet("outdated/")]
    public async Task<IActionResult> GetOutdatedAsync([FromQuery] DateTime? period)
    {
        var tasks = await _manager.GetOutdatedTasksAsync(Id, period);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Teacher))]
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
        var result = false;
        if (groupId is null)
            result = await _manager.TryAddTaskInRepositoryAsync(Id, model);
        else
            result = await _manager.TryAddTaskForGroupAsync(Id, groupId.Value, model);
        return result ? Ok() : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("/{taskId:long}/for-group")]
    public async Task<IActionResult> AddForGroups([FromBody] IEnumerable<GroupApiModel> groups,[FromRoute] long taskId)
    {
        var result = await _manager.TryAddTaskForGroupsAsync(Id, taskId, groups);
        return result
            ? Ok()
            : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    // [HttpPost("/{taskId:long}/update")]
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
        // помимо статус кода было бы славно возвращать текст ошибки
        if (file.ContentType != "application/pdf")
            return BadRequest("тип фийла должен быть .pdf");
        var result = await _manager.UploadAnswerToTaskAsync(Id, taskId, file);
        return result 
            ? Ok() 
            : StatusCode(500);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("/{taskId:long}/download-answer")]
    public IActionResult DownloadAnswer([FromRoute] long taskId)
    {
        var fileAnswer = _manager.DownloadAnswer(Id, taskId);
        if (fileAnswer is null)
            return BadRequest();
        return File(fileAnswer.Content, fileAnswer.ContentType, fileAnswer.FileName);
    }

    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpGet("unchecked/")]
    public IActionResult GetUncheckedTasks()
    {
        
        // получаем вообще все непроверенные для учителя
        throw new NotImplementedException();
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