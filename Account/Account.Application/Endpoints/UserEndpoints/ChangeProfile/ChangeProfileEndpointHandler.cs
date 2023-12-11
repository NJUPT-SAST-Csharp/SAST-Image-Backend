using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Auth.Authentication;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.UserEndpoints.ChangeProfile
{
    public sealed class ChangeProfileEndpointHandler(
        IUserQueryRepository repository,
        IUnitOfWork unit
    ) : IAuthEndpointHandler<ChangeProfileRequest>
    {
        private readonly IUnitOfWork _unit = unit;
        private readonly IUserQueryRepository _repository = repository;

        public async Task<IResult> Handle(ChangeProfileRequest request, ClaimsPrincipal user)
        {
            user.TryFetchId(out long id);

            var targetUser = await _repository.GetUserByIdAsync(id);
            targetUser!.EditProfile(
                request.Nickname,
                request.Biography,
                Uri.TryCreate(request.Avatar, UriKind.Absolute, out var avatar) ? avatar : null,
                Uri.TryCreate(request.Avatar, UriKind.Absolute, out var header) ? header : null
            );

            await _unit.SaveChangesAsync();

            return Responses.NoContent;
        }
    }
}
