using Shared.Primitives.DomainEvent;

namespace SNS.Domain.Follows.Events
{
    public sealed record class FollowedDomainEvent(UserId Follower, UserId Following) : IDomainEvent
    {
        public DateTime At { get; } = DateTime.UtcNow;
    }
}
