using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.AddToRole;

public record AddUserToRoleCommand(string UserId, string Role) : IRequest<Unit>;
