using DesignAutomation.API.Features.Users.Mapping;
using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DesignAutomation.API.Features.Users.Queries.ListUsers;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, IReadOnlyList<UserViewModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ListUsersQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IReadOnlyList<UserViewModel>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);
        var result = new List<UserViewModel>(users.Count);
        foreach (var user in users)
            result.Add(await user.ToViewModelAsync(_userManager));
        return result;
    }
}
