using Microsoft.AspNetCore.Http;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices;

public interface IImageStorageRepository
{
    public Task<ImageUrl> UploadImageAsync(
        IFormFile file,
        CancellationToken cancellationToken = default
    );

    public Task<Stream?> GetImageAsync(
        ImageId imageId,
        bool isThumbnail = false,
        CancellationToken cancellationToken = default
    );
}
