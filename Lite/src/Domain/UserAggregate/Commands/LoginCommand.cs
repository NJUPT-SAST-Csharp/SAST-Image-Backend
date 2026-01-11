using Domain.UserAggregate.Exceptions;
using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record LoginCommand(Username Username, PasswordInput Password) : ICommand<JwtToken>;

internal sealed class LoginCommandHandler(
    IUserRepository repository,
    IPasswordValidator validator,
    IJwtTokenGenerator jwtGenerator
) : ICommandHandler<LoginCommand, JwtToken>
{
    public async ValueTask<JwtToken> Handle(
        LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        var user =
            await repository.GetOrDefaultAsync(command.Username, cancellationToken)
            ?? throw new LoginException();

        return await user.LoginAsync(command, validator, jwtGenerator, cancellationToken);
    }
}
