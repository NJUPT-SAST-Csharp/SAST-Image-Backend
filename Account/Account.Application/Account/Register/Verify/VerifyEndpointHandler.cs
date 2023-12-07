using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyEndpointHandler : IEndpointHandler<VerifyRequest>
    {
        public async Task<IResult> Handle(VerifyRequest request)
        {
            return Responses.NoContent;
        }
    }
}
