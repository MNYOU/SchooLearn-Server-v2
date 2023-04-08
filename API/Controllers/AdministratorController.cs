using Dal.Enums;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = nameof(Role.Admin))]
[Route("administrator/")]
public class AdministratorController : ControllerBase
{
    private readonly IAdministratorManager _manager;

    public AdministratorController(IAdministratorManager manager)
    {
        _manager = manager;
    }

    [HttpGet("invitation-code")]
    public IActionResult GetInvitationCode()
    {
        var code = _manager.GetInvitationCode();
        return Ok(code);
    }


    [HttpPut("invitation-code/new")]
    public IActionResult GenerateNewInvitationCode()
    {
        var result = _manager.GenerateNewInvitationCode();
        return result ? Ok() : StatusCode(500);
    }
}