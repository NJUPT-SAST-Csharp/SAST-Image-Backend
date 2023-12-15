using Account.Application.SeedWorks;
using Account.Application.Services;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeEndpointHandler(IJwtProvider jwt, IAuthCodeCache cache)
        : IEndpointHandler<VerifyRegistrationCodeRequest>
    {
        private readonly IJwtProvider _jwt = jwt;
        private readonly IAuthCodeCache _cache = cache;

        public Task<IResult> Handle(VerifyRegistrationCodeRequest request)
        {
            var jwt = _jwt.GetRegistrantJwt(request.Email);

            _cache.DeleteCodeAsync(CodeCaheKey.Registration, request.Email);

            return Task.FromResult(Responses.Data(new VerifyRegistrationCodeDto(jwt)) as IResult);
        }
    }
}
