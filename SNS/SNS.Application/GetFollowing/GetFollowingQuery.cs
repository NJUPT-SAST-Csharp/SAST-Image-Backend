using Shared.Primitives.Query;
using SNS.Domain;

namespace SNS.Application.GetFollowing
{
    public sealed class GetFollowingQuery(long userId) : IQueryRequest<IEnumerable<FollowingDto>>
    {
        public UserId UserId { get; } = new(userId);
    }
}
