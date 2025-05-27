using Identity;
using Mediator;

namespace SNS.Domain.Bookmarks.Events;

public sealed record class BookmarkedDomainEvent(UserId UserId, ImageId ImageId) : IDomainEvent
{
    public DateTime At { get; } = DateTime.UtcNow;
}
