namespace TransLingo.Application.Authentication.Interfaces;
public interface IGoogleAuthService
{
   Task LoginAsync(string idToken);
}