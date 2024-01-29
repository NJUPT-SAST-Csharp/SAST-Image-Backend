using System.Text.Json.Serialization;

namespace SastImg.Application.ImageServices.GetImage
{
    public sealed class DetailedImageDto
    {
        [JsonConstructor]
        private DetailedImageDto() { }

        public long ImageId { get; init; }
        public string Title { get; init; }
        public DateTime UploadedAt { get; init; }
        public bool IsRemoved { get; init; }
        public long[] Tags { get; init; }
        public Uri Url { get; init; }
    }
}
