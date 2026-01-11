using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;

namespace Application.ImageServices;

public interface IImageAvailabilityChecker
{
    public Task<bool> CheckAsync(
        ImageId id,
        Actor actor,
        CancellationToken cancellationToken = default
    );
}
