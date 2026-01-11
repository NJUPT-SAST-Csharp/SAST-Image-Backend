using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;

namespace Application.AlbumServices;

public interface ICoverStorageManager
{
    public Task UpdateWithCustomImageAsync(
        AlbumId album,
        Stream stream,
        CancellationToken cancellationToken = default
    );

    public Task UpdateWithContainedImageAsync(
        AlbumId album,
        ImageId image,
        CancellationToken cancellationToken = default
    );

    public Task DeleteCoverAsync(AlbumId album, CancellationToken cancellationToken = default);

    public Stream? OpenReadStream(AlbumId album);
}
