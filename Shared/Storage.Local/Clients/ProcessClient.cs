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
            bool overwrite = false,
            CancellationToken cancellationToken = default
        )
        {
            if (File.Exists(key) == false)
                throw new FileNotFoundException(null, key);

            string target;

            if (overwrite)
                target = Path.ChangeExtension(key, ".webp");
            else
                target = Path.Combine(
                    Path.GetDirectoryName(key)!,
                    Path.GetFileNameWithoutExtension(key) + "_compressed.webp"
                );

            using var image = SKBitmap.Decode(key);
            using var data = image.PeekPixels();
            using var encoded = data.Encode(SKEncodedImageFormat.Webp, 50);
            await using var targetFile = File.OpenWrite(target);
            encoded.SaveTo(targetFile);

            if (overwrite)
                File.Delete(key);

            return new Uri(target);
        }
    }
}
