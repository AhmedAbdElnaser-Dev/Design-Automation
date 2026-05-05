using MediatR;

namespace DesignAutomation.API.Features.Users.Commands.ChangePassword;

public record ChangeUserPasswordCommand(string Id, string CurrentPassword, string NewPassword) : IRequest<Unit>;
