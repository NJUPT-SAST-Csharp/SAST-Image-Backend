using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Account.Application.Services;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;

public sealed class VerifyRegistrationCodeCommandHandler(IAuthCodeCache cache)
    : ICommandHandler<VerifyRegistrationCodeCommand, IResult>
{
    public async ValueTask<IResult> Handle(
        VerifyRegistrationCodeCommand request,
        CancellationToken cancellationToken
    )
    {
        bool result = await cache.VerifyCodeAsync(
            CodeCacheKey.Registration,
            request.Email,
            request.Code,
            cancellationToken
        );

        return Results.Ok(new VerifyRegistrationCodeDto(result));
    }
}
