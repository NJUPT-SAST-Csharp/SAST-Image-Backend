using System.Text.Json.Serialization;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public sealed class RemovedAlbumDto
    {
        [JsonConstructor]
        private RemovedAlbumDto() { }

        public long AlbumId { get; init; }
        public string Title { get; init; }
        public Uri CoverUri { get; init; }
    }
}
