using Account.Application.SeedWorks;
using Account.Application.Services;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyRequestHandler(IAuthCache cache) : IEndpointHandler<VerifyRequest>
    {
        private readonly IAuthCache _cache = cache;

        public async Task<IResult> Handle(VerifyRequest request)
        {
            await _cache.VerifyCodeAsync(request.Email, request.Code.ToString());
            return Results.NoContent();
        }
    }
}
