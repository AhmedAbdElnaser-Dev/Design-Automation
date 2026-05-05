using DesignAutomation.API.Common.Exceptions;
using DesignAutomation.API.Features.Users.Mapping;
using DesignAutomation.API.Features.Users.ViewModels;
using DesignAutomation.API.Models;
using DesignAutomation.API.Services;
using DesignAutomation.API.Services.Aps;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DesignAutomation.API.Features.Auth.Commands.AutodeskSignIn;

public class AutodeskSignInCommandHandler : IRequestHandler<AutodeskSignInCommand, AuthViewModel>
{
    private const string DefaultRole = "User";
    private const string ProviderName = "autodesk";

    private readonly IApsOAuthService _oauth;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AutodeskSignInCommandHandler(
        IApsOAuthService oauth,
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService)
    {
        _oauth = oauth;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthViewModel> Handle(AutodeskSignInCommand request, CancellationToken cancellationToken)
    {
        var token = await _oauth.ExchangeCodeAsync(request.Code, cancellationToken);
        var info = await _oauth.GetUserInfoAsync(token.AccessToken, cancellationToken);

        if (string.IsNullOrEmpty(info.Sub))
            throw new AuthenticationException("Autodesk did not return a user identifier.");

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.AutodeskUserId == info.Sub, cancellationToken);

        if (user is null && !string.IsNullOrEmpty(info.Email))
        {
            user = await _userManager.FindByEmailAsync(info.Email);
            if (user is not null)
            {
                user.AutodeskUserId = info.Sub;
                if (string.IsNullOrEmpty(user.AuthProvider))
                    user.AuthProvider = ProviderName;
                var linkResult = await _userManager.UpdateAsync(user);
                if (!linkResult.Succeeded)
                    throw new BadRequestException("Failed to link Autodesk account.", IdentityErrorsToDictionary(linkResult));
            }
        }

        if (user is null)
        {
            user = new ApplicationUser
            {
                Email = info.Email,
                UserName = info.PreferredUsername ?? info.Email ?? $"autodesk-{info.Sub}",
                FullName = info.Name,
                AutodeskUserId = info.Sub,
                AuthProvider = ProviderName,
                EmailConfirmed = info.EmailVerified,
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                throw new BadRequestException("Failed to create Autodesk-linked user.", IdentityErrorsToDictionary(createResult));

            await _userManager.AddToRoleAsync(user, DefaultRole);
        }

        var (jwt, expiresAt) = await _tokenService.CreateTokenAsync(user);
        return new AuthViewModel
        {
            Token = jwt,
            ExpiresAt = expiresAt,
            User = await user.ToViewModelAsync(_userManager),
        };
    }

    private static Dictionary<string, string[]> IdentityErrorsToDictionary(IdentityResult result)
        => result.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());
}
