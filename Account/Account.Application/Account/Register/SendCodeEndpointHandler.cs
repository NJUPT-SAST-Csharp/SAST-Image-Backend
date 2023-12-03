using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.User.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Account.Register
{
    public sealed class SendCodeEndpointHandler(IAuthCache cache, IUserCheckRepository repository)
        : IEndpointHandler<SendCodeRequest>
    {
        private readonly IAuthCache _cache = cache;

        public async Task<IResult
        // Results<NoContent, BadRequest<BadRequestResponse>>
        > Handle(SendCodeRequest request)
        {
            var code = Random.Shared.Next(100000, 999999).ToString();
            _ = _cache.StoreCodeAsync(request.Email, code);
            return Responses.NoContent;
        }
    }
}
