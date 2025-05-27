using Account.Application.Services;
using Account.Domain.UserEntity;
using Account.Domain.UserEntity.Services;
using Mediator;
using Microsoft.AspNetCore.Http;
using Primitives;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;

public sealed class CreateAccountCommandHandler(
    IAuthCodeCache cache,
    IUserRepository repository,
    IJwtProvider provider,
    IUnitOfWork unit
) : ICommandHandler<CreateAccountCommand, IResult>
{
    public async ValueTask<IResult> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = User.CreateNewUser(request.Username, request.Password, request.Email);

        await repository.AddNewUserAsync(user, cancellationToken);

        string jwt = provider.GetLoginJwt(user.Id, user.Username, user.Roles);

        await unit.CommitChangesAsync(cancellationToken);

        await cache.DeleteCodeAsync(CodeCacheKey.Registration, request.Email, cancellationToken);

        return Responses.Data(new CreateAccountDto(jwt));
    }
}
