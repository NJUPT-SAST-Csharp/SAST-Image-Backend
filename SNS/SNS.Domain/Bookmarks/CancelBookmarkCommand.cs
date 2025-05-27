using Identity;
using Mediator;

namespace SNS.Domain.Bookmarks;

public sealed class CancelBookmarkCommand(UserId userId, ImageId imageId) : ICommand
{
    public UserId UserId { get; } = userId;
    public ImageId ImageId { get; } = imageId;
}
