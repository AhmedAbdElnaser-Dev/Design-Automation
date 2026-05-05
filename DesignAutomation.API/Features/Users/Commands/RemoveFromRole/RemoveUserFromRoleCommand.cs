using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.RemoveFromRole;

public record RemoveUserFromRoleCommand(string UserId, string Role) : IRequest<Unit>;
