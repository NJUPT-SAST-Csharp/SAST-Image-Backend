using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Auth.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.UserEndpoints.ChangeProfile
{
    public sealed class ChangeProfileEndpointHandler(
        IUserQueryRepository repository,
        IUnitOfWork unit,
        ILogger<ChangeProfileEndpointHandler> logger
    ) : IAuthEndpointHandler<ChangeProfileRequest>
    {
        private readonly IUnitOfWork _unit = unit;
        private readonly IUserQueryRepository _repository = repository;
        private readonly ILogger<ChangeProfileEndpointHandler> _logger = logger;

        public async Task<IResult> Handle(ChangeProfileRequest request, ClaimsPrincipal user)
        {
            user.TryFetchId(out long id);

            var targetUser = await _repository.GetUserDetailByIdAsync(id);

            if (targetUser is null)
            {
                _logger.LogError("Couldn't find user & profile with id [{id}]", id);
                return Responses.BadRequest("Something went wrong.");
            }

            targetUser.EditProfile(
                request.Nickname,
                request.Biography,
                Uri.TryCreate(request.Avatar, UriKind.Absolute, out var avatar) ? avatar : null,
                Uri.TryCreate(request.Header, UriKind.Absolute, out var header) ? header : null
            );

            await _unit.SaveChangesAsync();

            _logger.LogInformation("User [{username}] has changed profile.", targetUser.Username);
            return Responses.NoContent;
        }
    }
}
