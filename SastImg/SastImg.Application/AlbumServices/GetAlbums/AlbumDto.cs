using System.Text.Json.Serialization;

namespace SastImg.Application.AlbumServices.GetAlbums;

public sealed class AlbumDto
{
    [JsonConstructor]
    private AlbumDto() { }

    public long AlbumId { get; init; }
    public long AuthorId { get; init; }
    public long CategoryId { get; init; }
    public long? CoverId { get; init; }
    public required string Title { get; init; }
}
