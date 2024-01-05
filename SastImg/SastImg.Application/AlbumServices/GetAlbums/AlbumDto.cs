using System.Text.Json.Serialization;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public sealed class AlbumDto
    {
        [JsonConstructor]
        private AlbumDto() { }

        public long AlbumId { get; init; }
        public long CategoryId { get; init; }
        public string Title { get; init; }
        public Uri CoverUri { get; init; }
    };
}
