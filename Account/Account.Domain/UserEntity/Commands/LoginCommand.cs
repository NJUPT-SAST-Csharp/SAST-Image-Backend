using Account.Domain.UserEntity.ValueObjects;
using Mediator;

namespace Account.Domain.UserEntity.Commands;

public sealed record class LoginCommand(Username Username, PasswordInput Password)
    : ICommand<JwtToken>;
