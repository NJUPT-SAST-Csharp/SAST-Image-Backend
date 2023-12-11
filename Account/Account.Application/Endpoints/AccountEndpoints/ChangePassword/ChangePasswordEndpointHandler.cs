using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Auth.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword
{
    public sealed class ChangePasswordEndpointHandler(
        IUserQueryRepository repository,
        IPasswordHasher hasher,
        IUnitOfWork unit,
        ILogger<ChangePasswordEndpointHandler> logger
    ) : IAuthEndpointHandler<ChangePasswordRequest>
    {
        private readonly ILogger<ChangePasswordEndpointHandler> _logger = logger;
        private readonly IUserQueryRepository _repository = repository;
        private readonly IPasswordHasher _hasher = hasher;
        private readonly IUnitOfWork _unit = unit;

        public async Task<IResult> Handle(ChangePasswordRequest request, ClaimsPrincipal user)
        {
            if (user.TryFetchId(out long id) == false)
            {
                _logger.LogError("Couldn't resolve UserId from ClaimsPrincipal.");
                return Responses.BadRequest("Something went wrong.");
            }

            var targetUser = await _repository.GetUserByIdAsync(id);

            if (targetUser is null)
            {
                _logger.LogError("Couldn't find user with id [{id}] from database.", id);
                return Responses.BadRequest("Something went wrong.");
            }

            var formerPasswordHash = await _hasher.HashAsync(request.FormerPassword);
            var newPasswordHash = await _hasher.HashAsync(request.NewPassword);
            var result = targetUser.ChangePassword(formerPasswordHash, newPasswordHash);

            if (result == false)
            {
                return Responses.BadRequest("Incorrect former password.");
            }

            _ = _unit.SaveChangesAsync();
            _logger.LogInformation(
                "User [{username}] password has been changed.",
                targetUser.Username
            );
            return Responses.NoContent;
        }
    }
}
