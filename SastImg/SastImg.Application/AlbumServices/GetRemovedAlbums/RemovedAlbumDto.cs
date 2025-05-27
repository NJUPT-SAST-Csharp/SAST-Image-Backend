using System.Text.Json.Serialization;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums;

public sealed class RemovedAlbumDto
{
    [JsonConstructor]
    private RemovedAlbumDto() { }

    public long AlbumId { get; init; }
    public long? CoverId { get; init; }
    public required string Title { get; init; }
}
