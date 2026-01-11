using Domain.AlbumAggregate.AlbumEntity;

namespace Application.AlbumServices;

public interface IAlbumModelRepository
{
    public Task<AlbumModel> GetAsync(AlbumId id, CancellationToken cancellationToken);

    public Task AddAsync(AlbumModel model, CancellationToken cancellationToken);
}
