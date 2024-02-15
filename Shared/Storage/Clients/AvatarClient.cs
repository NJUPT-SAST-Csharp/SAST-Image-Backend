using Aliyun.OSS;
using Microsoft.Extensions.Options;
using Shared.Storage.Implements;
using SNS.Application.UserServices.UpdateAvatar;
using SNS.Domain.UserEntity;
using Storage.Options;

namespace Storage.Clients
{
    internal sealed class AvatarClient(
        IOssClientFactory factory,
        IOptions<AvatarOssOptions> options
    ) : IAvatarStorageClient
    {
        private readonly OssClient _client = factory.GetOssClient();
        private readonly AvatarOssOptions _options = options.Value;

        public async Task<Uri> UploadAvatarAsync(
            UserId userId,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            string key = userId.Value.ToString();

            IAsyncResult start(AsyncCallback callBack, object? o) =>
                _client.BeginPutObject(_options.BucketName, key, stream, callBack, o);
            PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

            await Task.Factory.FromAsync(start, end, null);

            return new Uri(GetImageUrl(key));
        }

        private string GetImageUrl(string fileName)
        {
            return _options.Endpoint.Insert(
                    _options.Endpoint.IndexOf('/') + 2,
                    _options.BucketName + '.'
                ) + $"/{fileName}";
        }
    }
}
