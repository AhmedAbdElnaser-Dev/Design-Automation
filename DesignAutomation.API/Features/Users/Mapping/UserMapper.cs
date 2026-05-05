using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Mapping;

public static class UserMapper
{
    public static async Task<UserViewModel> ToViewModelAsync(this ApplicationUser user, UserManager<ApplicationUser> userManager)
    {
        var roles = await userManager.GetRolesAsync(user);
        return new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Roles = roles.ToArray(),
        };
    }
}
