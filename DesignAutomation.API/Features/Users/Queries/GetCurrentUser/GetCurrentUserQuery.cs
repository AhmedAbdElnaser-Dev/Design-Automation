using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<UserViewModel>;
