using Identity;
using Mediator;

namespace SNS.Application.GetFollowing;

public sealed class GetFollowingQuery(long userId) : IQuery<IEnumerable<FollowingDto>>
{
    public UserId UserId { get; } = new(userId);
}
