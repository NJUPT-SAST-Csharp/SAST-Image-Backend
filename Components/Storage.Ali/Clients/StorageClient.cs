using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using Storage.Options;

namespace Storage.Clients;

internal sealed class StorageClient(StorageOptions options) : IStorageClient
{
    private readonly OssClient _client = new(
        options.Endpoint,
        options.AccessKeyId,
        options.AccessKeySecret
    );
    private readonly StorageOptions _options = options;

    public Task DeleteImagesAsync(
        IEnumerable<Uri> keys,
        CancellationToken cancellationToken = default
    )
    {
        DeleteObjectsRequest request = new(
            _options.BucketName,
            keys.Select(key => key.AbsoluteUri).ToList(),
            true
        );

        return Task.Run(() => _client.DeleteObjects(request), cancellationToken);
    }

    public async Task<Stream?> GetImageAsync(Uri url, CancellationToken cancellationToken = default)
    {
        string key = url.AbsoluteUri;

        IAsyncResult start(AsyncCallback callBack, object? o) =>
            _client.BeginGetObject(_options.BucketName, key, callBack, o);

        OssObject end(IAsyncResult result) => _client.EndGetObject(result);

        var result = await Task.Factory.FromAsync(start, end, null).WaitAsync(cancellationToken);

        return result.Content;
    }

    public async Task<Uri> UploadImageAsync(
        IFormFile file,
        string key,
        CancellationToken cancellationToken = default
    )
    {
        await using var stream = file.OpenReadStream();

        IAsyncResult start(AsyncCallback callBack, object? o) =>
            _client.BeginPutObject(_options.BucketName, key, stream, callBack, o);

        PutObjectResult end(IAsyncResult result) => _client.EndPutObject(result);

        var result = await Task.Factory.FromAsync(start, end, null).WaitAsync(cancellationToken);

        string url = _options.GetUrl(key);

        return new Uri(url);
    }
}
