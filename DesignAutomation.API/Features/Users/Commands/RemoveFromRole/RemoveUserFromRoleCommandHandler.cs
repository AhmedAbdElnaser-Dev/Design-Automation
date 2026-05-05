using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Commands.RemoveFromRole;

public class RemoveUserFromRoleCommandHandler : IRequestHandler<RemoveUserFromRoleCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RemoveUserFromRoleCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
                  ?? throw new NotFoundException("User", request.UserId);

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
        if (!result.Succeeded)
            throw new BadRequestException("Role removal failed.",
                result.Errors.GroupBy(e => e.Code).ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()));

        return Unit.Value;
    }
}
