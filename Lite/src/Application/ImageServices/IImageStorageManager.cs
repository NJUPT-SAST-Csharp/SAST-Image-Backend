using Domain.AlbumAggregate.ImageEntity;

namespace Application.ImageServices;

public interface IImageStorageManager
{
    public Task StoreImageAsync(
        ImageId imageId,
        Stream imageFile,
        CancellationToken cancellationToken = default
    );

    public Stream? OpenReadStream(ImageId image, ImageKind kind);

    public Task DeleteImageAsync(ImageId image, CancellationToken cancellationToken = default);
}
