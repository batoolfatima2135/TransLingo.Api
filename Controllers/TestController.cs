using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransLingo.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("protected")]
    [Authorize]
    public IActionResult Protected()
    {
        var userId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier
        )?.Value;

        return Ok(new
        {
            message = "You are authenticated!",
            userId
        });
    }
}