using Microsoft.AspNetCore.Http;
using Storage.Options;

namespace Storage.Clients;

internal sealed class StorageClient(StorageOptions options) : IStorageClient
{
    private readonly StorageOptions _options = options;

    public Task DeleteImagesAsync(
        IEnumerable<Uri> urls,
        CancellationToken cancellationToken = default
    )
    {
        var fileNotExist = urls.FirstOrDefault(url => File.Exists(url.AbsolutePath) == false);
        if (fileNotExist is not null)
            throw new FileNotFoundException(null, fileNotExist.AbsolutePath);

        foreach (var url in urls)
        {
            File.Delete(url.AbsolutePath);
        }

        return Task.CompletedTask;
    }

    public Task<Stream?> GetImageAsync(Uri url, CancellationToken cancellationToken = default)
    {
        string path = url.AbsolutePath;

        if (File.Exists(path) == false)
            return Task.FromResult<Stream?>(null);

        FileStream file = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        return Task.FromResult<Stream?>(file);
    }

    public async Task<Uri> UploadImageAsync(
        IFormFile file,
        string key,
        CancellationToken cancellationToken = default
    )
    {
        string filename = _options.GetPath(key);

        Directory.CreateDirectory(Path.GetDirectoryName(filename)!);

        var fileStream = File.Create(filename);

        await file.CopyToAsync(fileStream, cancellationToken);

        await fileStream.DisposeAsync();

        return new Uri(filename);
    }
}
