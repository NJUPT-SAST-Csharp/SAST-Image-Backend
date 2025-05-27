using Identity;

namespace SNS.Domain.Bookmarks;

internal sealed record class Bookmark
{
    public required UserId UserId { get; init; }
    public required ImageId ImageId { get; init; }
}
