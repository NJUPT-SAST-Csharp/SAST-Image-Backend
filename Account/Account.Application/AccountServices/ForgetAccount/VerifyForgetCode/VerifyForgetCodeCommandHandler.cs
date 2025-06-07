using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;

public sealed class VerifyForgetCodeCommandHandler(
    IJwtProvider provider,
    IUserRepository repository
) : ICommandHandler<VerifyForgetCodeCommand, IResult>
{
    public async ValueTask<IResult> Handle(
        VerifyForgetCodeCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserByEmailAsync(request.Email, cancellationToken);

        string jwt = provider.GetLoginJwt(user.Id, user.Username, user.UserRoles);

        return Results.Ok(new VerifyForgetCodeDto(jwt));
    }
}
