namespace SastImg.Application.AlbumServices.GetAlbums
{
    public sealed class AlbumDto
    {
        private AlbumDto() { }

        public required long AlbumId { get; init; }
        public required long CategoryId { get; init; }
        public required string Title { get; init; }
        public required Uri CoverUri { get; init; }
    };
}
