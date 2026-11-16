namespace TransLingo.Api.Services.Google.Models;

public class GoogleUserInfo
{  public string Subject { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Picture { get; set; }
}