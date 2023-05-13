using Logic.ApiModels;
using Logic.Interfaces;
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
    public IActionResult LogIn([FromBody] LoginApiModel model)
    {
        var responseModel = _manager.GetAuthorizedModel(model);

        return responseModel is null
            ? Unauthorized()
            : Ok(responseModel);
    }
}