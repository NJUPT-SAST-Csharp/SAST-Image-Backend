using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Mediator;

namespace Account.Application.Commands;

public sealed class LoginCommandHandler(
    IUserRepository repository,
    IPasswordValidator validator,
    IJwtTokenGenerator generator
) : ICommandHandler<LoginCommand, JwtToken>
{
    public async ValueTask<JwtToken> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetByUsernameAsync(request.Username, cancellationToken);

        return await user.LoginAsync(request, validator, generator);
    }
}
