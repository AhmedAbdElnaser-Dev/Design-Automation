using DesignAutomation.API.Features.Users.ViewModels;
using MediatR;

namespace DesignAutomation.API.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<UserViewModel>;
