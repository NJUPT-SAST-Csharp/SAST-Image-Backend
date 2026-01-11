using Domain.AlbumAggregate.AlbumEntity;

namespace Domain.AlbumAggregate.Services;

public interface IAlbumTitleUniquenessChecker
{
    public Task CheckAsync(AlbumTitle title, CancellationToken cancellationToken = default);
}
