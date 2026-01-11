using Application.ImageServices;
using Domain.AlbumAggregate.ImageEntity;
using Infrastructure.Shared.Storage;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageStorageManager(
    ICompressProcessor compressor,
    IOptions<StorageOptions> options
) : StorageManagerBase(options.Value.BasePath), IImageStorageManager
{
    private const int CompressRate = 60;
    private readonly string CompressedFilename = "compressed.webp";

    public Task DeleteImageAsync(ImageId image, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(image, cancellationToken);
    }

    public Stream? OpenReadStream(ImageId image, ImageKind kind)
    {
        string mask = kind switch
        {
            ImageKind.Original => "original.*",
            ImageKind.Thumbnail => "compressed.*",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

        return OpenReadStream(image, mask);
    }

    public async Task StoreImageAsync(
        ImageId imageId,
        Stream imageFile,
        CancellationToken cancellationToken = default
    )
    {
        string extension = GetExtension(imageFile);

        string originalFilename = $"original.{extension}";

        await using var compressedImageFile = await compressor.CompressAsync(
            imageFile,
            CompressRate,
            cancellationToken
        );

        await Task.WhenAll(
            StoreAsync(imageFile, imageId, originalFilename, cancellationToken),
            StoreAsync(compressedImageFile, imageId, CompressedFilename, cancellationToken)
        );
    }

    private static string GetExtension(Stream file)
    {
        using var stream = new SKFrontBufferedManagedStream(
            file,
            SKCodec.MinBufferedBytesNeeded,
            false
        );

        long position = file.Position;
        try
        {
            using var codec = SKCodec.Create(stream);
            string format = codec.EncodedFormat.ToString().ToLowerInvariant();
            return format;
        }
        finally
        {
            file.Position = position;
        }
    }
}
