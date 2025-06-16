using Account.Domain.UserEntity.ValueObjects;
using Identity;
using Mediator;

namespace Account.Domain.UserEntity.Commands;

public sealed record class ChangePasswordCommand(PasswordInput NewPassword, Requester Requester)
    : ICommand;
