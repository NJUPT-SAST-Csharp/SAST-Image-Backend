using Application.AlbumServices;
using Application.ImageServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Infrastructure.Shared.Storage;
using Microsoft.Extensions.Options;

namespace Infrastructure.AlbumServices.Application;

internal sealed class CoverStorageManager(
    ICompressProcessor compressor,
    IImageStorageManager images,
    IOptions<StorageOptions> options
) : StorageManagerBase(options.Value.BasePath), ICoverStorageManager
{
    private const string filename = "cover.webp";
    private const int compressRate = 60;

    public Stream? OpenReadStream(AlbumId album)
    {
        return OpenReadStream(album, filename);
    }

    public Task DeleteCoverAsync(AlbumId album, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(album, cancellationToken);
    }

    public async Task UpdateWithContainedImageAsync(
        AlbumId album,
        ImageId image,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = images.OpenReadStream(image, ImageKind.Thumbnail);

        // TODO: Unimplemented logic in concurrent condition.
        if (stream == null)
            return;

        await StoreAsync(stream, album, filename, cancellationToken);
    }

    public async Task UpdateWithCustomImageAsync(
        AlbumId album,
        Stream stream,
        CancellationToken cancellationToken = default
    )
    {
        await using var _ = stream;
        await using var compressed = await compressor.CompressAsync(
            stream,
            compressRate,
            cancellationToken
        );

        await StoreAsync(compressed, album, filename, cancellationToken);
    }
}
