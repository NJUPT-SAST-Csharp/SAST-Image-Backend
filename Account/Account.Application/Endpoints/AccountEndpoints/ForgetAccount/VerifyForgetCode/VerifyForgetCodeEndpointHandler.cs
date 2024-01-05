using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeEndpointHandler(
        IAuthCodeCache cache,
        IUserQueryRepository repository,
        ILogger<VerifyForgetCodeEndpointHandler> logger
    ) : IEndpointHandler<VerifyForgetCodeRequest>
    {
        private readonly ILogger<VerifyForgetCodeEndpointHandler> _logger = logger;
        private readonly IUserQueryRepository _repository = repository;
        private readonly IAuthCodeCache _cache = cache;

        public async Task<IResult> Handle(VerifyForgetCodeRequest request)
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                _logger.LogError("Couldn't find user with email [{email}].", request.Email);
                return Responses.BadRequest("Something went wrong.");
            }

            await _cache.DeleteCodeAsync(CodeCacheKey.Registration, request.Email);

            var code = Random.Shared.Next(100000, 999999);

            await _cache.StoreCodeAsync(CodeCacheKey.ForgetAccount, request.Email, code);

            return Responses.Data(new VerifyForgetCodeDto(user.Username, code));
        }
    }
}
