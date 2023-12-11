using Account.Application.SeedWorks;
using Account.Application.Services;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeEndpointHandler(ITokenSender sender, IAuthCache cache)
        : IEndpointHandler<SendForgetCodeRequest>
    {
        private readonly ITokenSender _sender = sender;
        private readonly IAuthCache _cache = cache;

        public async Task<IResult> Handle(SendForgetCodeRequest request)
        {
            var code = Random.Shared.Next(100000, 999999);
            var result = await _sender.SendTokenAsync(code.ToString(), request.Email);
            if (result == false)
            {
                return Responses.BadRequest("Something went wrong.");
            }
            _ = _cache.StoreCodeAsync(CacheKeys.ForgetAccount, request.Email, code);
            return Responses.NoContent;
        }
    }
}
