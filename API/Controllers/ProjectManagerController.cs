using Dal.Enums;
using Dal.Repositories;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = $"{nameof(Role.ProjectManager)},{nameof(Role.SuperManager)}")]
[Route("manager/")]
public class ProjectManagerController: ControllerBase
{
    private readonly IProjectManager _manager;
    
    public ProjectManagerController(IProjectManager manager)
    {
        _manager = manager;
    }
    
    [Authorize(Roles = nameof(Role.ProjectManager))]
    [HttpGet("applications/")]
    public IActionResult GetAllApplications()
    {
        var applications = _manager.GetNotReviewedApplications();
        return Ok(applications);
    }
    
    [Authorize(Roles = nameof(Role.ProjectManager))]
    [HttpPut("consider/{applicationId:long}/{isConfirmed:bool}")]
    public IActionResult Consider([FromRoute] long applicationId, [FromRoute] bool isConfirmed)
    {
        var result = _manager.ConsiderApplication(applicationId, isConfirmed);
        return result ? Ok() : StatusCode(500);
    }

    [HttpGet("institution/{institutionId:long}/primary-code")]
    public IActionResult GetPrimaryCode([FromRoute] long institutionId, [FromServices] IInstitutionRepository institutionRepository)
    {
        var institution = institutionRepository.Institutions
            .FirstOrDefault(i => i.Id == institutionId);
        if (institution is null) return BadRequest("не найдено");
        return Ok(new { Code = institution.PrimaryInvitationCode });
    }
}