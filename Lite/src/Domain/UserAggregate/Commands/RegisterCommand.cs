using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record RegisterCommand(
    Username Username,
    Nickname Nickname,
    PasswordInput Password,
    RegistryCode Code
) : ICommand<JwtToken>;

internal sealed class RegisterCommandHandler(
    IRegistryCodeChecker codeChecker,
    IUsernameUniquenessChecker usernameChecker,
    IPasswordGenerator pwdGenerator,
    IJwtTokenGenerator jwtGenerator,
    IUserRepository repository
) : ICommandHandler<RegisterCommand, JwtToken>
{
    public async ValueTask<JwtToken> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await User.RegisterAsync(
            command,
            usernameChecker,
            codeChecker,
            pwdGenerator,
            jwtGenerator,
            repository,
            cancellationToken
        );

        return result;
    }
}
