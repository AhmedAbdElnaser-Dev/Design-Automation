using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.Login;

public record LoginUserCommand(string EmailOrUserName, string Password) : IRequest<AuthViewModel>;
