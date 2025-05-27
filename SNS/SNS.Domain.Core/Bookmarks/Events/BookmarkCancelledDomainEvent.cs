using Identity;
using Mediator;

namespace SNS.Domain.Bookmarks.Events;

public sealed record class BookmarkCancelledDomainEvent(UserId UserId, ImageId ImageId)
    : IDomainEvent
{
    public DateTime At { get; } = DateTime.UtcNow;
}
