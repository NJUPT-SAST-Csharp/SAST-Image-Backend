using Primitives.Command;

namespace SNS.Domain.Follows
{
    public sealed class FollowCommand(UserId follower, UserId target) : ICommandRequest
    {
        public UserId Follower { get; } = follower;
        public UserId Following { get; } = target;
    }
}
