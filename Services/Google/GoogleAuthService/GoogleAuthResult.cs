using System.Security.Claims;

namespace TransLingo.Api.Services;

public class GoogleAuthResult
{
    public ClaimsPrincipal? Principal { get; init; }

    public string? ErrorCode { get; init; }

    public bool Succeeded => Principal != null;
}