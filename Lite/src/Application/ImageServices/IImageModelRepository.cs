using Domain.AlbumAggregate.ImageEntity;

namespace Application.ImageServices;

public interface IImageModelRepository
{
    public Task<ImageModel> GetAsync(ImageId id, CancellationToken cancellationToken);

    public Task AddAsync(ImageModel image, CancellationToken cancellationToken);

    public Task DeleteAsync(ImageId id, CancellationToken cancellationToken);
}
