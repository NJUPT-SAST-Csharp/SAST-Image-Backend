using Identity;
using Mediator;

namespace SNS.Domain.Follows;

public sealed class UnfollowCommand(UserId follower, UserId following) : ICommand
{
    public UserId Follower { get; } = follower;

    public UserId Following { get; } = following;
}
