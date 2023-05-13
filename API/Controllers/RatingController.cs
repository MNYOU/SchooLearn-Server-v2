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

    public RatingController(IRatingManager manager)
    {
        _manager = manager;
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