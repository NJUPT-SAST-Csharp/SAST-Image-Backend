using System.Text.Json.Serialization;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.GetDetailedAlbum
{
    public sealed class DetailedAlbumDto
    {
        [JsonConstructor]
        private DetailedAlbumDto() { }

        public long AlbumId { get; init; }
        public long AuthorId { get; init; }
        public long CategoryId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public Accessibility Accessibility { get; init; }
        public DateTime UpdatedAt { get; init; }
        public bool IsRemoved { get; init; }
        public Uri CoverUri { get; init; }
        public long[] Collaborators { get; init; }
    }
}
