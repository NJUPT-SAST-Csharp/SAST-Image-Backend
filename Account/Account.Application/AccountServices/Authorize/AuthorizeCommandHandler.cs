using Account.Domain.RoleEntity.Services;
using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeCommandHandler(
        IUserRepository users,
        IRoleRespository roles,
        IUnitOfWork unit
    ) : ICommandRequestHandler<AuthorizeCommand>
    {
        private readonly IUserRepository _users = users;
        private readonly IRoleRespository _roles = roles;
        private readonly IUnitOfWork _unit = unit;

        public async Task Handle(AuthorizeCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetUserByIdAsync(request.UserId, cancellationToken);
            var role = await _roles.GetRoleByIdAsync(request.RoleId, cancellationToken);

            user.AddRole(role);

            await _unit.CommitChangesAsync(cancellationToken);
        }
    }
}
