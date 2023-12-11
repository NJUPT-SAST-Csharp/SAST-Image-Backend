using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.RoleEntity.Repositories;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeEndpointHandler(
        IUserQueryRepository users,
        IRoleRespository roles,
        IUnitOfWork unit
    ) : IAuthEndpointHandler<AuthorizeRequest>
    {
        private readonly IUserQueryRepository _users = users;
        private readonly IRoleRespository _roles = roles;
        private readonly IUnitOfWork _unit = unit;

        public async Task<IResult> Handle(AuthorizeRequest request, ClaimsPrincipal user)
        {
            var targetUser = await _users.GetUserByIdAsync(request.UserId);
            var role = await _roles.GetRoleByIdAsync(request.RoleId);

            if (targetUser is null || role is null)
            {
                return Responses.BadRequest("Invalid request.");
            }

            targetUser.AddRole(role);
            await _unit.SaveChangesAsync();
            return Responses.NoContent;
        }
    }
}
