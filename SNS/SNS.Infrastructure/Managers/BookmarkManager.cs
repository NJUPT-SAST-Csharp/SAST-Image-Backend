using Microsoft.EntityFrameworkCore;
using SNS.Domain;
using SNS.Domain.Bookmarks;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.Managers
{
    internal sealed class BookmarkManager(SNSDbContext context) : IBookmarkManager
    {
        private readonly SNSDbContext _context = context;

        public async Task BookmarkAsync(
            UserId userId,
            ImageId imageId,
            CancellationToken cancellationToken = default
        )
        {
            await _context.Bookmarks.AddAsync(
                new Bookmark() { ImageId = imageId, UserId = userId },
                cancellationToken
            );
        }

        public Task CancelBookmarkAsync(
            Bookmark bookmark,
            CancellationToken cancellationToken = default
        )
        {
            _context.Bookmarks.Remove(bookmark);
            return Task.CompletedTask;
        }

        public async Task<Bookmark?> GetBookmarkAsync(
            UserId userId,
            ImageId imageId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Bookmarks.FirstOrDefaultAsync(
                b => b.UserId == userId && b.ImageId == imageId,
                cancellationToken
            );
        }
    }
}
