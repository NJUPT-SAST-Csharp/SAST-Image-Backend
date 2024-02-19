using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Storage.Options;

namespace Storage.Clients
{
    public sealed class HeaderClient(IOptions<HeaderOssOptions> options)
    {
        private readonly OssClient _client =
            new(options.Value.Endpoint, options.Value.AccessKeyId, options.Value.AccessKeySecret);
        private readonly HeaderOssOptions _options = options.Value;

        public async Task<Uri> UploadHeaderAsync(long userId, IFormFile file)
        {
            string key = "headers/" + userId.ToString() + Path.GetExtension(file.FileName);

            using (Stream stream = file.OpenReadStream())
            {
                IAsyncResult start(AsyncCallback callBack, object? o) =>
                    _client.BeginPutObject(_options.BucketName, key, stream, callBack, o);

                PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

                await Task.Factory.FromAsync(start, end, null);
            }

            return new Uri(GetImageUrl(key));
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
