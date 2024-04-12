using Primitives.Command;

namespace SNS.Domain.Follows
{
    public sealed class UnfollowCommand(UserId follower, UserId following) : ICommandRequest
    {
        public UserId Follower { get; } = follower;

        public UserId Following { get; } = following;
    }
}
