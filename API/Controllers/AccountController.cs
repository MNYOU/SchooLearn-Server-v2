using Logic.ApiModels;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("account/")]
public class AccountController : ControllerBase
{
    private readonly IAccountManager _manager;

    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");

    public AccountController(IAccountManager manager)
    {
        _manager = manager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationApiModel model, IStudentManager studentManager)
    {
        var result = await _manager.Register(model);
        return result ? Ok() : StatusCode(500);
    }

    [HttpPost("login")]
    public IActionResult LogIn([FromBody] LoginApiModel model,[FromServices] IInstitutionManager institutionManager)
    {
        var responseModel = _manager.GetAuthorizedModel(model, institutionManager);

        return responseModel is null
            ? Unauthorized()
            : Ok(responseModel);
    }

    [HttpPut("rename")]
    [Authorize]
    public IActionResult RenameUser([FromQuery] string name)
    {
        var result = _manager.Rename(Id, name);
        return result ? Ok() : StatusCode(500);
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage()
    {
        // не реализованно
        var emailManager = new EmailManager();
        emailManager.SendMessageAsync("","", "");
        return Ok();
    }
}