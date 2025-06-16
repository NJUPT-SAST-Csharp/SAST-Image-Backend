using Account.Domain.UserEntity.ValueObjects;
using Identity;
using Mediator;

namespace Account.Domain.UserEntity.Commands;

public sealed record class UpdateUsernameCommand(Username Username, Requester Requester) : ICommand;
