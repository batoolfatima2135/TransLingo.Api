using TransLingo.Backend.DTOs.Auth;

namespace TransLingo.Api.Services.Interfaces;

public interface IGoogleAuthService
{
    Task<GoogleAuthResult> AuthenticateAsync(
        GoogleLoginRequest request);
}