using Dal.Enums;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("institution/")]
public class InstitutionController: ControllerBase
{
    private readonly IInstitutionManager _manager;
    private readonly IStudentManager _studentManager;
    
    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");

    public InstitutionController(IInstitutionManager manager, IStudentManager studentManager)
    {
        _manager = manager;
        _studentManager = studentManager;
    }
    
    [AllowAnonymous]
    [HttpPost("create/")]
    public async Task<IActionResult> CreateApplication([FromBody] InstitutionApiModel model)
    {
        var result = await _manager.CreateApplication(model);
        return result != null ? Ok() : StatusCode(500);
    }

    [AllowAnonymous]
    [HttpGet("confirmed/")]
    public IActionResult GetAllConfirmed()
    {
        var institutions = _manager.GetAllConfirmed();
        return Ok(institutions);
    }

    [HttpGet("{institutionId:long}/students/all")]
    public IActionResult GetAllStudents([FromRoute] long institutionId)
    {
        var students = _studentManager.GetAllByInstitution(Id, institutionId);
        if (students is null) return StatusCode(403);
        return Ok(students);
    }

    [HttpGet("{institutionId:long}/students/search")]
    public IActionResult SearchStudentsByNickname([FromRoute] long institutionId, [FromQuery] string nickname)
    {
        var students = _studentManager.SearchByNickname(Id, institutionId, nickname);
        if (students is null) return StatusCode(403);
        return Ok(students);
    }
    
}