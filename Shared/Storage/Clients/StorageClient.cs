using Aliyun.OSS;
using Storage.Options;

namespace Storage.Clients
{
    internal sealed class StorageClient(StorageOptions options) : IStorageClient
    {
        private readonly OssClient _client =
            new(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
        private readonly StorageOptions _options = options;

        public async Task<Uri> UploadImageAsync(
            Stream file,
            string key,
            CancellationToken cancellationToken = default
        )
        {
            IAsyncResult start(AsyncCallback callBack, object? o) =>
                _client.BeginPutObject(_options.BucketName, key, file, callBack, o);

            PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

            var result = await Task.Factory.FromAsync(start, end, null);

            var url = _options.GetUrl(key);

            return new Uri(url);
        }
    }
}
