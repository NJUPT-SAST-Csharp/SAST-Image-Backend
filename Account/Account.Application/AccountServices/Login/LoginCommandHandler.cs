using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Mediator;
using Microsoft.AspNetCore.Http;
using Primitives;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Login;

public sealed class LoginCommandHandler(
    IUserRepository repository,
    IJwtProvider jwtProvider,
    IUnitOfWork unit
) : ICommandHandler<LoginCommand, IResult>
{
    public async ValueTask<IResult> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserByUsernameAsync(request.Username, cancellationToken);

        bool isValid = await user.LoginAsync(request.Password);

        if (isValid == false)
        {
            return Responses.BadRequest("Login failed", "Username or password is incorrect.");
        }

        string jwt = jwtProvider.GetLoginJwt(user.Id, user.Username, user.UserRoles);

        await unit.CommitChangesAsync(cancellationToken);

        return Responses.Data(new LoginDto(jwt));
    }
}
