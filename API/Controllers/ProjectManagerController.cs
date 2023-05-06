using Dal.Enums;
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
}