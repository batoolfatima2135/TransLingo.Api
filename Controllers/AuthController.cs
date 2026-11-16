
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransLingo.Api.Models;
using TransLingo.Api.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;

namespace TransLingo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return BadRequest(new
            {
                message = "An account with this email already exists."
            });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, request.Password);

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
    var user = await _userManager.GetUserAsync(User);

    if (user == null)
    {
        return Unauthorized();
    }

    return Ok(new
    {
        userId = user.Id,
        email = user.Email,
        profilePictureUrl = user.ProfilePictureUrl,
        firstName = user.FirstName,
        lastName = user.LastName
    });
    }
}
