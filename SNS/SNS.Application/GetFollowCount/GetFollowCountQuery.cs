using Identity;
using Mediator;

namespace SNS.Application.GetFollowCount;

public sealed class GetFollowCountQuery(long id) : IQuery<FollowCountDto>
{
    public UserId UserId { get; } = new(id);
}
