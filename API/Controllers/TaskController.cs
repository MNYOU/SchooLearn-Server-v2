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

    [HttpGet("get/{id:long}")]
    public IActionResult Get([FromRoute] long id)
    {
        var task = _manager.Get(id);
        if (task is null)
            return BadRequest();
            // return StatusCode(204);
        return Ok(task);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("current/")]
    public async Task<IActionResult> GetCurrentForStudent()
    {
        var tasks = await _manager.GetCurrentForStudentAsync(Id);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("solved/all")]
    public async Task<IActionResult> GetSolvedTasks()
    {
        var tasks = await _manager.GetSolvedTasksAsync(Id);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("solved/{id:long}")]
    public IActionResult GetSolvedTask([FromRoute] long taskId)
    {
        var task = _manager.GetSolvedTask(Id, taskId);
        if (task != null)
            return BadRequest();
        return Ok(task);
    }
    

    [Authorize(nameof(Role.Teacher))]
    [HttpGet("assigned/")]
    public async Task<IActionResult> GetAssigned()
    {
        var tasks = await _manager.GetAssignedTasksAsync(Id);
        return Ok(tasks);
    }

    [Authorize(nameof(Role.Teacher))]
    [HttpGet("students-completed-tasks/")]
    public IActionResult GetStudentsCompetedTasks([FromQuery] long taskId)
    { 
        var students = _manager.GetStudentsWhoCompletedTask(Id, taskId);
        return Ok(students);
    }


    [Authorize(Roles = nameof(Role.Teacher))]
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] TaskApiModel model, [FromQuery] long groupId)
    {
        // TODO как сделать добавление нескольких групп
        if (model.IsExtended != null && !model.IsExtended.Value && model.Answer is null or "")
            return BadRequest("Ответ не может быть null, если задание не является расширенным");
        var result = await _manager.TryAddTaskForGroupAsync(model, Id, groupId);
        return result ? Ok() : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpPost("check/")]
    public IActionResult Check([FromBody] TaskWithAnswerRequest model)
    {
        var result = _manager.CheckAndPointTask(model, Id);
        return result ? Ok() : StatusCode(500);
    }

    [Authorize(Roles = nameof(Role.Student))]
    [HttpPost("upload-answer/")]
    public async Task<IActionResult> UploadAnswer([FromBody] IFormFile file, [FromQuery] long taskId)
    {
        // помимо статус кода было бы славно возвращать текст ошибки
        if (file.ContentType != "application/pdf")
            return BadRequest("тип фийла должен быть .pdf");
        var result = await _manager.UploadAnswerToTaskAsync(Id, taskId, file);
        return result ? Ok() : StatusCode(500);
    }

    [Authorize(nameof(Role.Student))]
    [HttpGet("download-answer")]
    public IActionResult DownloadAnswer(long taskId)
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