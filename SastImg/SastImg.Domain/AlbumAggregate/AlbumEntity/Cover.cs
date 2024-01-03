namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    public sealed record class Cover(Uri? Url, bool IsLatestImage) { };
}
