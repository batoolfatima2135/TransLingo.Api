using Microsoft.AspNetCore.Identity;

namespace TransLingo.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string? ProfilePictureUrl { get; set; }
}