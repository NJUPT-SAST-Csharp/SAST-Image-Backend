using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    public sealed class DetailedAlbumDto
    {
        private DetailedAlbumDto() { }

        public required long AlbumId { get; init; }
        public required long AuthorId { get; init; }
        public required long CategoryId { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required Accessibility Accessibility { get; init; }
        public required DateTime UpdatedAt { get; init; }
        public required Uri CoverUri { get; init; }
        public required bool IsRemoved { get; init; }
        public required IEnumerable<long> Collaborators { get; init; }
    }
}
