using Shared.Primitives.Query;
using SNS.Domain;

namespace SNS.Application.GetFollowers
{
    public sealed class GetFollowersQuery(long userId) : IQueryRequest<IEnumerable<FollowerDto>>
    {
        public UserId UserId { get; } = new(userId);
    }
}
