using Aliyun.OSS;
using Microsoft.Extensions.Options;
using SastImg.Application.ImageServices.AddImage;
using Shared.Storage.Options;
using System.Text;

namespace Shared.Storage.Implements
{
    internal sealed class ImageClient(IOssClientFactory factory, IOptions<OssOptions> options)
        : IImageStorageClient
    {
        private readonly OssClient _client = factory.GetOssClient();
        private readonly OssOptions _options = options.Value;

        public async Task<Uri> UploadImageAsync(
            string fileName,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            string key = Path.GetRandomFileName() + Path.GetExtension(fileName);

            IAsyncResult start(AsyncCallback callBack, object? o) =>
                _client.BeginPutObject(_options.ImageBucketName, key, stream, callBack, o);
            PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

            await Task.Factory.FromAsync(start, end, null);
            await CompressImageAsync(key, cancellationToken);

            return new Uri(GetImageUrl(key));
        }

        private async Task CompressImageAsync(
            string originalFileName,
            CancellationToken cancellationToken = default
        )
        {
            var targetFileName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(Path.ChangeExtension(originalFileName, ".webp"))
            );
            var targetBucketName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(_options.ImageBucketName)
            );

            ProcessObjectRequest request =
                new(_options.ImageBucketName, originalFileName)
                {
                    Process = $"style/compress|sys/saveas,o_{targetFileName},b_{targetBucketName}"
                };

            await Task.Factory.StartNew(() => _client.ProcessObject(request));
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
