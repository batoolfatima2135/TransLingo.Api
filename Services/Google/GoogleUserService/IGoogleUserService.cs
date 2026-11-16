using TransLingo.Api.Models;
using TransLingo.Api.Services.Google.Models;

namespace TransLingo.Api.Services.Interfaces;

public interface IGoogleUserService
{
    Task<ApplicationUser> FindOrCreateAsync(GoogleUserInfo userInfo);
}