namespace TransLingo.Api.DTOs.Auth;

public class CurrentUserResponse
{
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}