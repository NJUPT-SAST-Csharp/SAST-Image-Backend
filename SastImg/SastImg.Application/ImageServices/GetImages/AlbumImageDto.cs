using System.Text.Json.Serialization;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class AlbumImageDto
    {
        [JsonConstructor]
        private AlbumImageDto() { }

        public long ImageId { get; init; }
        public long AlbumId { get; init; }
        public string Title { get; init; }
    }
}
