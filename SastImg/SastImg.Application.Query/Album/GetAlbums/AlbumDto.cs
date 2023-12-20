using SastImg.Domain.Albums;

namespace SastImg.Application.Query.Album.GetAlbums
{
    public sealed class AlbumDto
    {
        public required long AuthorId { get; init; }
        public required long AlbumId { get; init; }
        public required string Title { get; init; }
        public required Accessibility Accessibility { get; init; }
        public required Uri CoverUri { get; init; }
    }
}
