using Identity;
using Mediator;

namespace SNS.Domain.Follows;

public sealed class FollowCommand(UserId follower, UserId target) : ICommand
{
    public UserId Follower { get; } = follower;
    public UserId Following { get; } = target;
}
