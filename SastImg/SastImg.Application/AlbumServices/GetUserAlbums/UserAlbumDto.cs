using System.Text.Json.Serialization;

namespace SastImg.Application.AlbumServices.GetUserAlbums;

public sealed class UserAlbumDto
{
    [JsonConstructor]
    private UserAlbumDto() { }

    public long AlbumId { get; init; }
    public long CategoryId { get; init; }
    public long? CoverId { get; init; }
    public required string Title { get; init; }
};
