namespace Storage.Clients
{
    public interface IProcessClient
    {
        public Task<Uri> ProcessImageAsync(
            string key,
            CancellationToken cancellationToken = default
        );

        public Task<string> GetExtensionNameAsync(
            Stream file,
            CancellationToken cancellationToken = default
        );
    }
}
