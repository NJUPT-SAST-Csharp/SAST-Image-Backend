using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SastImg.Application.ImageServices.AddImage;
using Shared.Storage.Implements;
using Shared.Storage.Options;
using System.Text;

namespace Storage.Clients
{
    internal sealed class ImageClient(IOssClientFactory factory, IOptions<ImageOssOptions> options)
        : IImageStorageClient
    {
        private readonly OssClient _client = factory.GetOssClient();
        private readonly ImageOssOptions _options = options.Value;

        public async Task<Uri> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            string key =
                "images/"
                + DateTime.UtcNow.ToString("yyyyMMdd")
                + Path.GetRandomFileName().Replace(".", "")
                + Path.GetExtension(file.FileName);

            using (Stream stream = file.OpenReadStream())
            {
                IAsyncResult start(AsyncCallback callBack, object? o) =>
                    _client.BeginPutObject(_options.BucketName, key, stream, callBack, o);

                PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

                await Task.Factory.FromAsync(start, end, null);
            }

            await CompressImageAsync(key, cancellationToken);

            return new Uri(GetImageUrl(key));
        }

        private async Task CompressImageAsync(
            string originalFileName,
            CancellationToken cancellationToken = default
        )
        {
            var targetFileName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    Path.ChangeExtension(originalFileName + "_thumbnail", ".webp")
                )
            );
            var targetBucketName = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(_options.BucketName)
            );

            ProcessObjectRequest request =
                new(_options.BucketName, originalFileName)
                {
                    Process = $"style/compress|sys/saveas,o_{targetFileName},b_{targetBucketName}"
                };

            await Task.Factory.StartNew(() => _client.ProcessObject(request));
        }

        private string GetImageUrl(string key)
        {
            return _options.Endpoint.Insert(
                    _options.Endpoint.IndexOf('/') + 2,
                    _options.BucketName + '.'
                ) + $"/{key}";
        }
    }
}
