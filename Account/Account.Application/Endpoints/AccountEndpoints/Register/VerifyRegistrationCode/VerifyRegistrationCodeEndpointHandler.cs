using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeEndpointHandler
        : IEndpointHandler<VerifyRegistrationCodeRequest>
    {
        public Task<IResult> Handle(VerifyRegistrationCodeRequest request)
        {
            return Task.FromResult(Responses.NoContent as IResult);
        }
    }
}
