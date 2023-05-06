using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("rating/")]
public class RatingController: ControllerBase
{
    // TODO рейтинг
    [HttpGet("global")]
    public IActionResult GetGlobalRating()
    {
        throw new NotImplementedException();
    }
}