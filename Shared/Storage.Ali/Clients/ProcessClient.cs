using System.Text;
using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using Storage.Options;

namespace Storage.Clients
{
    internal sealed class ProcessClient(StorageOptions options) : IProcessClient
    {
        private readonly OssClient _client =
            new(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
        private readonly StorageOptions _options = options;

        public async ValueTask<string> GetExtensionNameAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            await using var stream = file.OpenReadStream();
            var format = await Image.DetectFormatAsync(stream, cancellationToken);
            return format.FileExtensions.First();
        }

        public Task<Uri> CompressImageAsync(
            string key,
            bool overwrite = false,
            CancellationToken cancellationToken = default
        )
        {
            string newKey;
            if (overwrite)
                newKey = Path.ChangeExtension(key, ".webp");
            else
                newKey = Path.Combine(
                    Path.GetDirectoryName(key)!,
                    Path.GetFileNameWithoutExtension(key) + "_compressed.webp"
                );

            var targetFileName = Convert.ToBase64String(Encoding.UTF8.GetBytes(newKey));
            var targetBucketName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(_options.BucketName)
            );

            const string style = "image/auto-orient,1/quality,q_50/format,webp";

            ProcessObjectRequest request =
                new(_options.BucketName, key)
                {
                    Process = $"{style}|sys/saveas,o_{targetFileName},b_{targetBucketName}"
                };

            _ = Task.Run(
                () =>
                {
                    _client.ProcessObject(request);
                    if (overwrite)
                        _client.DeleteObject(_options.BucketName, key);
                },
                cancellationToken
            );

            var url = _options.GetUrl(newKey);

            return Task.FromResult(new Uri(url));
        }
    }
}
