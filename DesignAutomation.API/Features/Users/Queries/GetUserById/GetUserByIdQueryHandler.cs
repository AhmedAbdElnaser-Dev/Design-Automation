using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Features.Users.Mapping;
using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id)
                  ?? throw new NotFoundException("User", request.Id);

        return await user.ToViewModelAsync(_userManager);
    }
}
