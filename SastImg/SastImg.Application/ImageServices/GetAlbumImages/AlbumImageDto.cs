using System.Text.Json.Serialization;

namespace SastImg.Application.ImageServices.GetAlbumImages;

public sealed class AlbumImageDto
{
    [JsonConstructor]
    private AlbumImageDto() { }

    public long ImageId { get; init; }
    public long AlbumId { get; init; }
    public required string Title { get; init; }
}
