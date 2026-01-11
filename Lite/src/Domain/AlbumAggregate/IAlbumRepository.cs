using Domain.AlbumAggregate.AlbumEntity;

namespace Domain.AlbumAggregate;

internal interface IAlbumRepository
{
    public Task<Album> GetAsync(AlbumId id, CancellationToken cancellationToken);

    public Task AddAsync(Album album, CancellationToken cancellationToken);
}
