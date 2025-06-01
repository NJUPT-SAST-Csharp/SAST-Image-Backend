namespace Storage.Client;

public interface IProcessClient
{
    public Task<Uri> CompressImageAsync(
        string key,
        bool overwrite = false,
        CancellationToken cancellationToken = default
    );
}
