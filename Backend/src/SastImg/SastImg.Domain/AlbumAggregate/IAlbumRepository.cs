using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Domain.AlbumAggregate;

public interface IAlbumRepository
{
    public Task<Album> GetAlbumAsync(AlbumId id, CancellationToken cancellationToken = default);

    public Task<AlbumId> AddAlbumAsync(Album album, CancellationToken cancellationToken = default);
}
