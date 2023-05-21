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
    public async Task<IActionResult> CreateApplication([FromBody] InstitutionApiRequest request, [FromServices] IProjectManager projectManager)
    {
        var result = await _manager.CreateApplication(request, projectManager);
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
    public IActionResult GetAllStudents([FromRoute] long institutionId, [FromServices] IAccountManager accountManager)
    {
        var students = _studentManager.GetAllByInstitution(Id, institutionId, accountManager);
        if (students is null) return StatusCode(403);
        return Ok(students);
    }

    [HttpGet("{institutionId:long}/students/search")]
    public IActionResult SearchStudentsByNickname([FromRoute] long institutionId, [FromQuery] string nickname, [FromServices] IAccountManager accountManager)
    {
        var students = _studentManager.SearchByNickname(Id, institutionId, nickname, accountManager);
        if (students is null) return StatusCode(403);
        return Ok(students);
    }
}