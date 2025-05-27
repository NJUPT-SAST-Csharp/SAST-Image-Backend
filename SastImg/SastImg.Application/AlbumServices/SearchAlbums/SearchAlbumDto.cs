using System.Text.Json.Serialization;

namespace SastImg.Application.AlbumServices.SearchAlbums;

public sealed class SearchAlbumDto
{
    [JsonConstructor]
    private SearchAlbumDto() { }

    public long AuthorId { get; init; }
    public long AlbumId { get; init; }
    public long CategoryId { get; init; }
    public long? CoverId { get; init; }
    public required string Title { get; init; }
}
