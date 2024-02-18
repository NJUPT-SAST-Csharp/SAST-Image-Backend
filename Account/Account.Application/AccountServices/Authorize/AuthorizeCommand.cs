using Account.Domain.UserEntity;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeCommand(long userId, Role[] roles) : ICommandRequest
    {
        public UserId UserId { get; } = new(userId);
        public Role[] Roles { get; } = roles;
    }
}
