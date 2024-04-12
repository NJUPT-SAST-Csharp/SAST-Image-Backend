namespace SNS.Domain.Bookmarks;

internal sealed record class Bookmark
{
    public UserId UserId { get; }
    public ImageId ImageId { get; }
}
