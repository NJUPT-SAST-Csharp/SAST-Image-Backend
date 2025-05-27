using Identity;

namespace SNS.Domain.Likes;

internal sealed record class Like
{
    public UserId UserId { get; }
    public ImageId ImageId { get; }
}
