using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Common.Identity;
using DesignAutomation.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Commands.ChangePassword;

public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUser;

    public ChangeUserPasswordCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId != request.Id)
            throw new ForbiddenException("You may only change your own password.");

        var user = await _userManager.FindByIdAsync(request.Id)
                  ?? throw new NotFoundException("User", request.Id);

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new BadRequestException("Password change failed.",
                result.Errors.GroupBy(e => e.Code).ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()));

        return Unit.Value;
    }
}
