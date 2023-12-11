using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    internal class VerifyForgetCodeEndpointHandler(
        IAuthCache cache,
        IUserQueryRepository repository,
        ILogger<VerifyForgetCodeEndpointHandler> logger
    ) : IEndpointHandler<VerifyForgetCodeRequest>
    {
        private readonly ILogger<VerifyForgetCodeEndpointHandler> _logger = logger;
        private readonly IUserQueryRepository _repository = repository;
        private readonly IAuthCache _cache = cache;

        public async Task<IResult> Handle(VerifyForgetCodeRequest request)
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                _logger.LogError("Couldn't find user with email [{email}].", request.Email);
                return Responses.BadRequest("Something went wrong.");
            }

            _ = _cache.DeleteCodeAsync(CacheKeys.Registration, request.Email);

            var code = Random.Shared.Next(100000, 999999);

            _ = _cache.StoreCodeAsync(CacheKeys.Registration, request.Email, code);

            return Responses.Data(new VerifyForgetCodeDto(user.Username, code));
        }
    }
}
