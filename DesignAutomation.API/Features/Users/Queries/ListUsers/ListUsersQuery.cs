using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Users.Queries.ListUsers;

public record ListUsersQuery() : IRequest<IReadOnlyList<UserViewModel>>;
