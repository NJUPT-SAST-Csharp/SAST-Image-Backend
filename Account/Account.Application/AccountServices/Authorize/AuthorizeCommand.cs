using Account.Domain.RoleEntity;
using Account.Domain.UserEntity;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeCommand(long userId, int roleId) : ICommandRequest
    {
        public UserId UserId { get; } = new(userId);
        public RoleId RoleId { get; } = new(roleId);
    }
}
