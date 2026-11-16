using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransLingo.Backend.DTOs.Auth;
using TransLingo.Api.Services.Interfaces;

namespace TransLingo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleAuthService _googleAuthService;

    public GoogleAuthController(IGoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin(
        [FromBody] GoogleLoginRequest request)
    {
        var result = await _googleAuthService.AuthenticateAsync(request);

        if (!result.Succeeded)
        {
            return Unauthorized(new { error = result.ErrorCode });
        }

        await HttpContext.SignInAsync(
            IdentityConstants.BearerScheme,
            result.Principal!);

       return new EmptyResult();
    }
}