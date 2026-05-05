using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string Id,
    string? UserName,
    string? Email,
    string? FullName,
    string? PhoneNumber) : IRequest<UserViewModel>;
