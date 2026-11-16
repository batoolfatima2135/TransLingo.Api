using Microsoft.AspNetCore.Identity;
using TransLingo.Api.DTOs.Auth;
using TransLingo.Api.Models;
using TransLingo.Api.Services.Interfaces;
using TransLingo.Backend.DTOs.Auth;

namespace TransLingo.Api.Services.Google;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly IGoogleTokenValidator _tokenValidator;
    private readonly IGoogleUserService _userService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public GoogleAuthService(
        IGoogleTokenValidator tokenValidator,
        IGoogleUserService userService,
        SignInManager<ApplicationUser> signInManager)
    {
        _tokenValidator = tokenValidator;
        _userService = userService;
        _signInManager = signInManager;
    }

    public async Task<GoogleAuthResult> AuthenticateAsync(
        GoogleLoginRequest request)
    {
        // 1. Validate the Google token
        var googleUserInfo = await _tokenValidator.ValidateAsync(
            request.IdToken);

        if (googleUserInfo == null)
        {
            return new GoogleAuthResult
            {
                ErrorCode = "InvalidGoogleToken"
            };
        }

        // 2. Ensure Google has verified the email
        if (!googleUserInfo.EmailVerified ||
            string.IsNullOrWhiteSpace(googleUserInfo.Email))
        {
            return new GoogleAuthResult
            {
                ErrorCode = "EmailNotVerified"
            };
        }

        // 3. Find or create the local TransLingo user
        var user = await _userService.FindOrCreateAsync(googleUserInfo);

        // 4. Create the user's authentication principal
        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        return new GoogleAuthResult
        {
            Principal = principal
        };
    }
}