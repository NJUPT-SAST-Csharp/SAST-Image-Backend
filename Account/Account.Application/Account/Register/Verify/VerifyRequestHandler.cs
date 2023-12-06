using Account.Application.SeedWorks;
using Account.Application.Services;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyRequestHandler(IAuthCache cache) : IEndpointHandler<VerifyRequest>
    {
        private readonly IAuthCache _cache = cache;

        public async Task<IResult> Handle(VerifyRequest request)
        {
            var result = await _cache.VerifyCodeAsync(request.Email, request.Code.ToString());

            if (result)
            {
                return Responses.BadRequest("Incorrect code.");
            }
            else
            {
                return Results.NoContent();
            }
        }
    }
}
