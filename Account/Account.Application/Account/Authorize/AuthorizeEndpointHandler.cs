using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.RoleEntity.Repositories;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Account.Authorize
{
    public sealed class AuthorizeEndpointHandler(
        IUserQueryRepository users,
        IRoleRespository roles,
        IUnitOfWork unit
    ) : IEndpointHandler<AuthorizeRequest>
    {
        private readonly IUserQueryRepository _users = users;
        private readonly IRoleRespository _roles = roles;
        private readonly IUnitOfWork _unit = unit;

        public async Task<IResult> Handle(AuthorizeRequest request)
        {
            var user = await _users.GetUserByIdAsync(request.UserId);
            var role = await _roles.GetRoleByIdAsync(request.RoleId);

            if (user is null || role is null)
            {
                return Responses.BadRequest("Invalid request.");
            }
            else
            {
                user.AddRole(role);
                await _unit.SaveChangesAsync();
                return Responses.NoContent;
            }
        }
    }
}
