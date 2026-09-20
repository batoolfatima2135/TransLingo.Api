namespace TransLingo.Application.Authentication.Models;
public record GoogleUserInfo(
    string GoogleId,
    string Email,
    string Name,
    string? Picture);