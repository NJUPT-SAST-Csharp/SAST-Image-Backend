using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Storage.Implements;
using SNS.Application.UserServices.UpdateHeader;
using SNS.Domain.UserEntity;
using Storage.Options;

namespace Storage.Clients
{
    internal sealed class HeaderClient(
        IOssClientFactory factory,
        IOptions<HeaderOssOptions> options
    ) : IHeaderStorageClient
    {
        private readonly OssClient _client = factory.GetOssClient();
        private readonly HeaderOssOptions _options = options.Value;

        public async Task<Uri> UploadHeaderAsync(
            UserId userId,
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            string key = "headers/" + userId.Value.ToString() + Path.GetExtension(file.FileName);

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
