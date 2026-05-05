using DesignAutomation.API.Features.Users.Commands.AddToRole;
using DesignAutomation.API.Features.Users.Commands.ChangePassword;
using DesignAutomation.API.Features.Users.Commands.DeleteUser;
using DesignAutomation.API.Features.Users.Commands.Login;
using DesignAutomation.API.Features.Users.Commands.Register;
using DesignAutomation.API.Features.Users.Commands.RemoveFromRole;
using DesignAutomation.API.Features.Users.Commands.UpdateUser;
using DesignAutomation.API.Features.Users.Queries.GetCurrentUser;
using DesignAutomation.API.Features.Users.Queries.GetUserById;
using DesignAutomation.API.Features.Users.Queries.ListUsers;
using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignAutomation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthViewModel>> Register(RegisterUserCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command, ct));

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthViewModel>> Login(LoginUserCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command, ct));

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> Me(CancellationToken ct)
        => Ok(await _mediator.Send(new GetCurrentUserQuery(), ct));

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IReadOnlyList<UserViewModel>>> List(CancellationToken ct)
        => Ok(await _mediator.Send(new ListUsersQuery(), ct));

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> GetById(string id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserByIdQuery(id), ct));

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> Update(string id, UpdateUserCommand command, CancellationToken ct)
        => Ok(await _mediator.Send(command with { Id = id }, ct));

    [HttpPost("{id}/change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string id, ChangeUserPasswordCommand command, CancellationToken ct)
    {
        await _mediator.Send(command with { Id = id }, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteUserCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id}/roles/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddToRole(string id, string role, CancellationToken ct)
    {
        await _mediator.Send(new AddUserToRoleCommand(id, role), ct);
        return NoContent();
    }

    [HttpDelete("{id}/roles/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveFromRole(string id, string role, CancellationToken ct)
    {
        await _mediator.Send(new RemoveUserFromRoleCommand(id, role), ct);
        return NoContent();
    }
}
