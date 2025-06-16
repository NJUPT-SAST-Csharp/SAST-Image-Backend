using Account.Domain.UserEntity;
using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Mediator;

namespace Account.Application.Commands;

public sealed class CreateAccountCommandHandler(
    IUserRepository repository,
    IPasswordGenerator passwordGenerator,
    IJwtTokenGenerator jwtGenerator,
    IUsernameUniquenessChecker checker
) : ICommandHandler<RegisterCommand, JwtToken>
{
    public async ValueTask<JwtToken> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await User.RegisterAsync(request, passwordGenerator, checker);

        await repository.AddAsync(user, cancellationToken);

        return jwtGenerator.Issue(user);
    }
}
