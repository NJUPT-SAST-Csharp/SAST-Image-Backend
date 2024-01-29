using System.Text.Json.Serialization;

namespace SastImg.Application.TagServices
{
    public sealed class TagDto
    {
        [JsonConstructor]
        private TagDto() { }

        public long Id { get; init; }
        public string Name { get; init; }
    }
}
