using Storage.Options;

namespace Storage.Clients
{
    internal sealed class StorageClient(StorageOptions options) : IStorageClient
    {
        private readonly StorageOptions _options = options;

        public Task DeleteImagesAsync(
            IEnumerable<string> keys,
            CancellationToken cancellationToken = default
        )
        {
            var fileNotExist = keys.FirstOrDefault(key => File.Exists(key) == false);
            if (fileNotExist is not null)
                throw new FileNotFoundException(null, fileNotExist);

            foreach (var key in keys)
            {
                File.Delete(key);
            }

            return Task.CompletedTask;
        }

        public async Task<Uri> UploadImageAsync(
            Stream file,
            string key,
            CancellationToken cancellationToken = default
        )
        {
            string filename = _options.GetUrl(key);

            await using var fileStream = File.Create(filename);
            await file.CopyToAsync(fileStream, cancellationToken);

            return new Uri(filename);
        }
    }
}
