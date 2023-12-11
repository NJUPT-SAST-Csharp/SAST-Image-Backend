using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword
{
    public sealed class ResetPasswordEndpointHandler(
        IAuthCache cache,
        IUnitOfWork unit,
        IUserQueryRepository repository,
        IPasswordHasher hasher,
        ILogger<ResetPasswordEndpointHandler> logger
    ) : IEndpointHandler<ResetPasswordRequest>
    {
        private readonly IAuthCache _cache = cache;
        private readonly IUserQueryRepository _repository = repository;
        private readonly IUnitOfWork _unit = unit;
        private readonly IPasswordHasher _hasher = hasher;
        private readonly ILogger<ResetPasswordEndpointHandler> _logger = logger;

        public async Task<IResult> Handle(ResetPasswordRequest request)
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                _logger.LogError("Couldn't find user with email [{email}]", request.Email);
                return Responses.BadRequest("Something went wrong.");
            }

            var passwordHash = await _hasher.HashAsync(request.Password);

            user.ResetPassword(passwordHash);

            _ = _unit.SaveChangesAsync();

            _ = _cache.DeleteCodeAsync(CacheKeys.ForgetAccount, request.Email);

            return Responses.NoContent;
        }
    }
}
