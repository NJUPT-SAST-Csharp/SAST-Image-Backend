using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum;

public sealed class CreateAlbumDto(AlbumId id)
{
    public long Id { get; } = id.Value;
}
