using System.Text.Json.Serialization;

namespace SastImg.Application.ImageServices.GetUserImages
{
    public sealed class UserImageDto
    {
        [JsonConstructor]
        private UserImageDto() { }

        public long ImageId { get; init; }
        public long AlbumId { get; init; }
        public string Title { get; init; }
        public Uri Url { get; init; }
    }
}
