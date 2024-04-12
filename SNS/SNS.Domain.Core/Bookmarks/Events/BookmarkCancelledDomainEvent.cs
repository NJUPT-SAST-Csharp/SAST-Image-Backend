using Shared.Primitives.DomainEvent;

namespace SNS.Domain.Bookmarks.Events
{
    internal sealed record class BookmarkCancelledDomainEvent(UserId UserId, ImageId ImageId)
        : IDomainEvent
    {
        public DateTime At { get; } = DateTime.UtcNow;
    }
}
