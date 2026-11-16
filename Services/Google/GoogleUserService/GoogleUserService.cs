using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using TransLingo.Api.Models;
using TransLingo.Api.Services.Google.Models;
using TransLingo.Api.Services.Interfaces;

namespace TransLingo.Api.Services.Google;

public class GoogleUserService : IGoogleUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GoogleUserService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> FindOrCreateAsync(
        GoogleUserInfo userInfo)
    
    {
        var user = await _userManager.FindByEmailAsync(userInfo.Email);

        user ??= await CreateUserFromGoogleUserInfo(userInfo);

        await UpdateUserProfileFromGoogleUserInfo(user, userInfo);

        return user;
    }

    private async Task<ApplicationUser> CreateUserFromGoogleUserInfo(GoogleUserInfo userInfo)
    {
      var newUser = new ApplicationUser
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                FirstName = userInfo.GivenName,
                LastName = userInfo.FamilyName,
                ProfilePictureUrl = userInfo.Picture,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create user from Google account.");
            }

            return newUser;
    }

    private async Task  UpdateUserProfileFromGoogleUserInfo(ApplicationUser user, GoogleUserInfo userInfo)
    {
        var needsUpdate = false;

        if (string.IsNullOrWhiteSpace(user.FirstName))
        {
            user.FirstName = userInfo.GivenName;
            needsUpdate = true;
        }

        if (string.IsNullOrWhiteSpace(user.LastName))
        {
            user.LastName = userInfo.FamilyName;
            needsUpdate = true;
        }

        if (string.IsNullOrWhiteSpace(user.ProfilePictureUrl))
        {
            user.ProfilePictureUrl = userInfo.Picture;
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not update Google user profile.");
            }
        }
    }
}