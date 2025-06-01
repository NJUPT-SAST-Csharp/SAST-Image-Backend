using Identity;
using Mediator;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize;

public sealed class AuthorizeCommand(long userId, Roles[] roles) : ICommand
{
    public UserId UserId { get; } = new() { Value = userId };
    public Roles[] Roles { get; } = roles;
}
