using SkiaSharp;

namespace Storage.Clients
{
    internal sealed class ProcessClient : IProcessClient
    {
        public ValueTask<string> GetExtensionNameAsync(
            Stream file,
            CancellationToken cancellationToken = default
        )
        {
            using SKCodec code = SKCodec.Create(file);
            return ValueTask.FromResult(code.EncodedFormat.ToString());
        }

        public async Task<Uri> CompressImageAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            if (File.Exists(key) == false)
                throw new FileNotFoundException(null, key);

            var target = Path.ChangeExtension(key, ".webp");

            using var image = SKBitmap.Decode(key);
            using var data = image.PeekPixels();
            using var encoded = data.Encode(SKEncodedImageFormat.Webp, 50);
            await using var targetFile = File.OpenWrite(target);
            encoded.SaveTo(targetFile);

            return new Uri(target);
        }
    }
}
