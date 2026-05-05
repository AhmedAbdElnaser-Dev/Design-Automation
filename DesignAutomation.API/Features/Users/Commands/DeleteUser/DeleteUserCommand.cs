using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<Unit>;
