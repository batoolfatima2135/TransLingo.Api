using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TransLingo.Api.DTOs.Auth;
using TransLingo.Api.Models;
using TransLingo.Api.Services.Interfaces;

namespace TransLingo.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

   public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
{
     if (await EmailAlreadyExistsAsync(request.Email))
    {
        return IdentityResult.Failed(
            new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "An account with this email already exists."
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

    return await _userManager.CreateAsync(user, request.Password);
}
    public async Task<CurrentUserResponse?> GetCurrentUserAsync(
        ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);

        if (user == null)
        {
            return null;
        }

        return new CurrentUserResponse
        {
            UserId = user.Id,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
    private async Task<bool> EmailAlreadyExistsAsync(string email)
{
    var existingUser = await _userManager.FindByEmailAsync(email);

    return existingUser != null;
}
}