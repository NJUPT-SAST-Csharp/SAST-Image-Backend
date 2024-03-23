namespace Storage.Clients
{
    public interface IStorageClient
    {
        public Task<Uri> UploadImageAsync(
            Stream file,
            string key,
            CancellationToken cancellationToken = default
        );
    }
}
