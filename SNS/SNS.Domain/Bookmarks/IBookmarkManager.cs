using Identity;

namespace SNS.Domain.Bookmarks;

internal interface IBookmarkManager
{
    Task<Bookmark?> GetBookmarkAsync(
        UserId userId,
        ImageId imageId,
        CancellationToken cancellationToken = default
    );
    Task BookmarkAsync(
        UserId userId,
        ImageId imageId,
        CancellationToken cancellationToken = default
    );
    Task CancelBookmarkAsync(Bookmark bookmark, CancellationToken cancellationToken = default);
}
