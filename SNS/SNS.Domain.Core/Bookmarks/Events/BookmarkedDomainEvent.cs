using Shared.Primitives.DomainEvent;

namespace SNS.Domain.Bookmarks.Events
{
    internal sealed record class BookmarkedDomainEvent(UserId UserId, ImageId ImageId)
        : IDomainEvent
    {
        public DateTime At { get; } = DateTime.UtcNow;
    }
}
