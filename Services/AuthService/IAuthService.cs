using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TransLingo.Api.DTOs.Auth;

namespace TransLingo.Api.Services.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterRequest request);

    Task<CurrentUserResponse?> GetCurrentUserAsync(
        ClaimsPrincipal principal);
}