using Domain.Extensions;
using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record class RefreshTokenCommand(RefreshToken RefreshToken) : ICommand<JwtToken> { }

internal sealed class RefreshTokenCommandHandler(
    IUserRepository repository,
    IJwtTokenGenerator generator
) : ICommandHandler<RefreshTokenCommand, JwtToken>
{
    public async ValueTask<JwtToken> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetAsync(command.RefreshToken.Id, cancellationToken);

        return user.RefreshToken(command, generator);
    }
}
