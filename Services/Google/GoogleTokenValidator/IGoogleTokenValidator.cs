using TransLingo.Api.Services.Google.Models;

namespace TransLingo.Api.Services.Interfaces;

public interface IGoogleTokenValidator
{
    Task<GoogleUserInfo?> ValidateAsync(string idToken);
}