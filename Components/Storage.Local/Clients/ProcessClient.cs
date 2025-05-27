using Microsoft.AspNetCore.Http;
using SkiaSharp;
using Storage.Options;

namespace Storage.Clients;

internal sealed class ProcessClient(StorageOptions options) : IProcessClient
{
    private readonly StorageOptions _options = options;

    public ValueTask<string> GetExtensionNameAsync(
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        using var stream = file.OpenReadStream();
        using var code = SKCodec.Create(stream);
        return ValueTask.FromResult(code.EncodedFormat.ToString().ToLowerInvariant());
    }

    public async Task<Uri> CompressImageAsync(
        string key,
        bool overwrite = false,
        CancellationToken cancellationToken = default
    )
    {
        string filename = _options.GetPath(key);

        if (File.Exists(filename) == false)
            throw new FileNotFoundException(null, filename);

        string target;

        if (overwrite)
            target = Path.ChangeExtension(filename, ".webp");
        else
            target = Path.Combine(
                Path.GetDirectoryName(filename)!,
                Path.GetFileNameWithoutExtension(filename) + "_compressed.webp"
            );
        {
            using var image = SKBitmap.Decode(filename);
            using var data = image.PeekPixels();
            using var encoded = data.Encode(SKEncodedImageFormat.Webp, 50);
            await using var targetFile = File.OpenWrite(target);
            encoded.SaveTo(targetFile);
        }

        if (overwrite && filename.EndsWith(".webp") == false)
            File.Delete(filename);

        return new Uri(target);
    }
}
