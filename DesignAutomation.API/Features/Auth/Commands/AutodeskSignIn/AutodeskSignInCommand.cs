using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Auth.Commands.AutodeskSignIn;

public record AutodeskSignInCommand(string Code) : IRequest<AuthViewModel>;
