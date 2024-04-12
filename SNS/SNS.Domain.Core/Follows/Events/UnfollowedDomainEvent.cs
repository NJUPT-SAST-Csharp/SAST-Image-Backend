using Shared.Primitives.DomainEvent;

namespace SNS.Domain.Core.Follows.Events
{
    public sealed record class UnfollowedDomainEvent(UserId FollowerId, UserId FollowingId)
        : IDomainEvent
    {
        public DateTime At { get; } = DateTime.UtcNow;
    }
}
