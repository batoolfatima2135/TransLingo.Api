
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransLingo.Api.Models;
using TransLingo.Api.DTOs.Auth;
using TransLingo.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TransLingo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController( IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterRequest request)
    {
        

        var result = await _authService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(error => error.Description)
            });
        }

        return Ok(new
        {
            message = "Account created successfully."
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
    var user = await _authService.GetCurrentUserAsync(User);

    if (user == null)
    {
        return Unauthorized();
    }

    return Ok(user);
    }
}
