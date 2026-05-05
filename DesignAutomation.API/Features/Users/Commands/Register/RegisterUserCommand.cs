using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.Register;

public record RegisterUserCommand(
    string Email,
    string Password,
    string? UserName,
    string? FullName,
    string? PhoneNumber) : IRequest<AuthViewModel>;
