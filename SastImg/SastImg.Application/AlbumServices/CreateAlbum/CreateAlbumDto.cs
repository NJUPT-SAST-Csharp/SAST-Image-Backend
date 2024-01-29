using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    public sealed class CreateAlbumDto(AlbumId id)
    {
        public AlbumId Id { get; } = id;
    }
}
