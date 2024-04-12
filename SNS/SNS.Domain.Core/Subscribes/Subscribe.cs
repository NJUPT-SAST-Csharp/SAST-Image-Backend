namespace SNS.Domain.Subscribes;

internal sealed record class Subscribe
{
    public UserId UserId { get; }
    public AlbumId AlbumId { get; }
}
