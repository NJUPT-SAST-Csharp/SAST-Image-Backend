using SkiaSharp;

namespace Infrastructure.Shared.Storage;

public interface ICompressProcessor
{
    public Task<Stream> CompressAsync(
        Stream originalFile,
        int rate,
        CancellationToken cancellationToken = default
    );
}

internal sealed class CompressProcessor : ICompressProcessor
{
    public Task<Stream> CompressAsync(
        Stream originalFile,
        int rate,
        CancellationToken cancellationToken = default
    )
    {
        return Task.Run(
            () =>
            {
                using var skStream = new SKFrontBufferedManagedStream(
                    originalFile,
                    SKFrontBufferedStream.DefaultBufferSize,
                    false
                );

                int position = skStream.Position;
                try
                {
                    using var image = SKBitmap.Decode(skStream);
                    using var data = image.PeekPixels();
                    var stream = data.Encode(SKEncodedImageFormat.Webp, rate)!.AsStream(true);
                    return stream;
                }
                finally
                {
                    originalFile.Position = position;
                }
            },
            cancellationToken
        );
    }
}
