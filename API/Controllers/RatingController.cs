using System.Security.Claims;
using Dal.Enums;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("rating/")]
public class RatingController : ControllerBase
{
    private readonly IRatingManager _manager;

    private long Id => long.Parse(User.FindFirst("Id")?.Value ?? "0");
    
    private Role UserRole
    {
        get
        {
            var value = User.FindFirst(ClaimTypes.Role)?.Value;
            return value != null ? Enum.Parse<Role>(value) : Role.Default;
        }
    }

    public RatingController(IRatingManager manager)
    {
        _manager = manager;
    }

    [HttpGet("my/")]
    public IActionResult GetMyRating()
    {
        if (UserRole != Role.Student) return StatusCode(403);
        return Ok(_manager.GetMyRating(Id));

    }

    [HttpGet("global/")]
    public IActionResult GetGlobalRating([FromQuery] long? institutionId)
    {
        var ratingApiModels = _manager.GetGlobal(Id, institutionId);
        return ratingApiModels != null
            ? Ok(ratingApiModels)
            : BadRequest();
    }

    [HttpGet("get/")]
    public IActionResult Get([FromQuery] long? institutionId, [FromQuery] long? groupId, [FromQuery] long? subjectId,
        [FromQuery] DateTime? from, [FromQuery] DateTime to)
    {
        var ratingApiModels = _manager.GetByCondition(Id, institutionId, groupId, subjectId, from, to);
        return ratingApiModels != null
            ? Ok(ratingApiModels)
            : BadRequest();
    }
}