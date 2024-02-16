using Shared.Primitives.Query;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.GetUser
{
    public sealed class GetUserQuery(long userId) : IQueryRequest<UserDto?>
    {
        public UserId UserId { get; } = new(userId);
    }
}
