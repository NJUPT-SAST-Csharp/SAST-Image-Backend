using System.Text;
using Aliyun.OSS;
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
            Stream file,
            CancellationToken cancellationToken = default
        )
        {
            var format = await Image.DetectFormatAsync(file, cancellationToken);
            return format.FileExtensions.First();
        }

        public Task<Uri> CompressImageAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            var builder = new StringBuilder(128);

            var index = key.LastIndexOf('.');

            var newKey = builder
                .Append(key)
                .Remove(index, builder.Length - index)
                .Append("_thumbnail")
                .Append(".webp")
                .ToString();

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

            _ = Task.Run(() => _client.ProcessObject(request), cancellationToken);

            var url = _options.GetUrl(newKey);

            return Task.FromResult(new Uri(url));
        }
    }
}
