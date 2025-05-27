using Identity;
using Mediator;

namespace SNS.Application.GetFollowers;

public sealed class GetFollowersQuery(long userId) : IQuery<IEnumerable<FollowerDto>>
{
    public UserId UserId { get; } = new(userId);
}
