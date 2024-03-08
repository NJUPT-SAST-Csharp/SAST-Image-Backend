using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Account.Application.Services;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeCommandHandler(IAuthCodeCache cache)
        : ICommandRequestHandler<VerifyRegistrationCodeCommand, IResult>
    {
        private readonly IAuthCodeCache _cache = cache;

        public async Task<IResult> Handle(
            VerifyRegistrationCodeCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _cache.VerifyCodeAsync(
                CodeCacheKey.Registration,
                request.Email,
                request.Code,
                cancellationToken
            );

            return Responses.Data(new VerifyRegistrationCodeDto(result));
        }
    }
}
