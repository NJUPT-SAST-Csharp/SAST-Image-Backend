using Identity;
using Mediator;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize;

public sealed class AuthorizeCommand(long userId, Role[] roles) : ICommand
{
    public UserId UserId { get; } = new() { Value = userId };
    public Role[] Roles { get; } = roles;
}
