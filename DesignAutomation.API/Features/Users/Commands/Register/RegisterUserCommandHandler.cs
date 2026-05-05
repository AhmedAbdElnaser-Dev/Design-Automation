using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Features.Users.Mapping;
using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using DesignAutomation.API.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthViewModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthViewModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName ?? request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new BadRequestException("User registration failed.", IdentityErrorsToDictionary(result));

        var (token, expiresAt) = await _tokenService.CreateTokenAsync(user);
        return new AuthViewModel
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = await user.ToViewModelAsync(_userManager),
        };
    }

    private static Dictionary<string, string[]> IdentityErrorsToDictionary(IdentityResult result)
        => result.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());
}
