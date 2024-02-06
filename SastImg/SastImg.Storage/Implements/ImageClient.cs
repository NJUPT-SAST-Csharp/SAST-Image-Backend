using System.Text;
using Aliyun.OSS;
using Microsoft.Extensions.Options;
using SastImg.Storage.Options;
using SastImg.Storage.Services;

namespace SastImg.Storage.Implements
{
    internal sealed class ImageClient(IOssClientFactory factory, IOptions<OssOptions> options)
        : IImageClient
    {
        private readonly OssClient _client = factory.GetOssClient();
        private readonly OssOptions _options = options.Value;

        public async Task<Uri> CompressImageAsync(
            string originalFileName,
            CancellationToken cancellationToken = default
        )
        {
            var targetFileName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(Path.GetFileNameWithoutExtension(originalFileName) + ".webp")
            );
            var targetBucketName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(_options.ThumbnailBucketName)
            );

            ProcessObjectRequest request =
                new(_options.ImageBucketName, originalFileName)
                {
                    Process = $"style/compress|sys/saveas,o_{targetFileName},b_{targetBucketName}"
                };

            await Task.Factory.StartNew(() => _client.ProcessObject(request));

            return new Uri(GetCompressedImageUrl(targetFileName));
        }

        public async Task<Uri> UploadImageAsync(
            string fileName,
            FileStream stream,
            CancellationToken cancellationToken = default
        )
        {
            IAsyncResult start(AsyncCallback callBack, object? o) =>
                _client.BeginPutObject(_options.ImageBucketName, fileName, stream, callBack, o);
            PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

            await Task.Factory.FromAsync(start, end, null);

            return new Uri(GetImageUrl(fileName));
        }

        private string GetCompressedImageUrl(string fileName)
        {
            return _options.Endpoint.Insert(
                    _options.Endpoint.IndexOf('/') + 2,
                    _options.ThumbnailBucketName + '.'
                ) + $"/{fileName}";
        }

        private string GetImageUrl(string fileName)
        {
            return _options.Endpoint.Insert(
                    _options.Endpoint.IndexOf('/') + 2,
                    _options.ImageBucketName + '.'
                ) + $"/{fileName}";
        }
    }
}
