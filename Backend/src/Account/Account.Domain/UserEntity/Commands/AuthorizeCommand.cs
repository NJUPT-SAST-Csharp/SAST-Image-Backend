using Identity;
using Mediator;

namespace Account.Domain.UserEntity.Commands;

public sealed record AuthorizeCommand(UserId UserId, Role[] Roles, Requester Requester) : ICommand;
