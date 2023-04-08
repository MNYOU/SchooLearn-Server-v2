using Dal.Enums;
using Logic.ApiModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("institution/")]
public class InstitutionController: ControllerBase
{
    private readonly IInstitutionManager _manager;

    public InstitutionController(IInstitutionManager manager)
    {
        _manager = manager;
    }
    
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateApplication([FromBody] InstitutionApiModel model)
    {
        var result = await _manager.CreateApplication(model);
        return result != null ? Ok() : StatusCode(500);
    }

    [AllowAnonymous]
    [HttpGet("confirmed")]
    public IActionResult GetAllConfirmed()
    {
        var institutions = _manager.GetAllConfirmed();
        return Ok(institutions);
    }

    [Authorize(Roles = nameof(Role.ProjectManager))]
    [HttpGet("applications")]
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