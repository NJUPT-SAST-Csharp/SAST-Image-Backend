namespace SastImg.Domain.AlbumAggregate
{
    public sealed record class Cover(Uri? Url, bool IsLatestImage) { };
}
