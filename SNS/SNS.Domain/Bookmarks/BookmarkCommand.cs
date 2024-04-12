using Primitives.Command;

namespace SNS.Domain.Bookmarks
{
    public sealed class BookmarkCommand(UserId userId, ImageId imageId) : ICommandRequest
    {
        public UserId UserId { get; } = userId;
        public ImageId ImageId { get; } = imageId;
    }
}
