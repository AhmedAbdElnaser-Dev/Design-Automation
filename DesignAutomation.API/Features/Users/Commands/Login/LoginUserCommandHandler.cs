using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Features.Users.Mapping;
using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using DesignAutomation.API.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DesignAutomation.API.Features.Users.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthViewModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailOrUserName)
                  ?? await _userManager.FindByNameAsync(request.EmailOrUserName);

        if (user is null)
            throw new AuthenticationException("Invalid credentials.");

        var check = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!check.Succeeded)
            throw new AuthenticationException("Invalid credentials.");

        var (token, expiresAt) = await _tokenService.CreateTokenAsync(user);
        return new AuthViewModel
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = await user.ToViewModelAsync(_userManager),
        };
    }
}
