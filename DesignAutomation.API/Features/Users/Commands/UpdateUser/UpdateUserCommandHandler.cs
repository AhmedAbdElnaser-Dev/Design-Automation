using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Common.Identity;
using DesignAutomation.API.Features.Users.Mapping;
using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserViewModel>
{
    private const string AdminRole = "Admin";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUser;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task<UserViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id)
                  ?? throw new NotFoundException("User", request.Id);

        if (_currentUser.UserId != request.Id && !_currentUser.IsInRole(AdminRole))
            throw new ForbiddenException("You may only update your own profile.");

        if (request.UserName is not null) user.UserName = request.UserName;
        if (request.Email is not null) user.Email = request.Email;
        if (request.PhoneNumber is not null) user.PhoneNumber = request.PhoneNumber;
        if (request.FullName is not null) user.FullName = request.FullName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new BadRequestException("User update failed.",
                result.Errors.GroupBy(e => e.Code).ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()));

        return await user.ToViewModelAsync(_userManager);
    }
}
