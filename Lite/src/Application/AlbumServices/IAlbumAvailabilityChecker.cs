using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices;

public interface IAlbumAvailabilityChecker
{
    public Task<bool> CheckAsync(AlbumId albumId, Actor actor, CancellationToken cancellationToken);
}
