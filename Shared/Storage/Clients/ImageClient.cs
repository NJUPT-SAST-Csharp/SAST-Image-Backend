using System.Text;
using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Storage.Options;

namespace Storage.Clients
{
    public sealed class ImageClient(IOptions<ImageOssOptions> options)
    {
        private readonly OssClient _client =
            new(options.Value.Endpoint, options.Value.AccessKeyId, options.Value.AccessKeySecret);
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

            await CompressImageAsync(key);

            return new Uri(GetImageUrl(key));
        }

        private async Task CompressImageAsync(string originalFileName)
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
