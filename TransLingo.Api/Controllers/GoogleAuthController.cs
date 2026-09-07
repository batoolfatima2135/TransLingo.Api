using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransLingo.Api.Models;

namespace TransLingo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class GoogleAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public GoogleAuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin(
        [FromBody] GoogleLoginRequest request)
    {
        try
        {
            // Get Google Client ID from User Secrets / configuration
            var clientId =
                _configuration["Authentication:Google:ClientId"];

            if (string.IsNullOrEmpty(clientId))
            {
                return StatusCode(500, new
                {
                    message = "Google Client ID is not configured."
                });
            }
             // 1. Validate the Google ID token
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                });
            // 2. Find the user by email
            var user = await _userManager.FindByEmailAsync(payload.Email);

            // 3. Create the user if they don't exist
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true,
                    ProfilePictureUrl = payload.Picture
                };

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                // Update profile picture if necessary
                user.ProfilePictureUrl = payload.Picture;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
             // 4. Create the ASP.NET Core Identity ClaimsPrincipal
            var principal =
                await _signInManager.CreateUserPrincipalAsync(user);

            // 5. Sign in using ASP.NET Core Identity's bearer scheme
            //
            // This is the same bearer authentication scheme
            // used by MapIdentityApi<ApplicationUser>().
            await HttpContext.SignInAsync(
                IdentityConstants.BearerScheme,
                principal);

            // The bearer-token handler has already written the
            // accessToken / refreshToken response.
            return new EmptyResult();
        }
        catch (InvalidJwtException)
        {
            return Unauthorized(new
            {
                message = "Invalid Google ID token."
            });
        }
    }
}

public record GoogleLoginRequest(string IdToken);