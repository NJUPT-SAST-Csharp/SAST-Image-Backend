using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword
{
    public sealed class ResetPasswordEndpointHandler(
        IAuthCodeCache cache,
        IUnitOfWork unit,
        IUserQueryRepository repository,
        ILogger<ResetPasswordEndpointHandler> logger
    ) : IEndpointHandler<ResetPasswordRequest>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IUserQueryRepository _repository = repository;
        private readonly IUnitOfWork _unit = unit;
        private readonly ILogger<ResetPasswordEndpointHandler> _logger = logger;

        public async Task<IResult> Handle(ResetPasswordRequest request)
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                _logger.LogError("Couldn't find user with email [{email}]", request.Email);
                return Responses.BadRequest("Something went wrong.");
            }

            user.ResetPassword(request.Password);

            _ = _unit.SaveChangesAsync();

            _ = _cache.DeleteCodeAsync(CodeCacheKey.ForgetAccount, request.Email);

            return Responses.NoContent;
        }
    }
}
