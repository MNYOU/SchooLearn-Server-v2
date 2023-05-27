using System.Security.Claims;
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

    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");

    public AdministratorController(IAdministratorManager manager)
    {
        _manager = manager;
    }

    [HttpGet("invitation-code")]
    public IActionResult GetInvitationCode()
    {
        var code = _manager.GetInvitationCode(Id);
        return code != null ? Ok(new { Code = code}) : StatusCode(500);
    }


    [HttpPut("invitation-code/new")]
    public IActionResult GenerateNewInvitationCode()
    {
        var result = _manager.GenerateNewInvitationCode(Id);
        return result ? Ok() : StatusCode(500);
    }
}