using Orleans;
using Orleans.Concurrency;

namespace Storage.Application.Contracts;

[Alias("Storage.Application.Contracts.IConfirmGrain")]
public interface IConfirmGrain : IGrainWithIntegerKey
{
    [Alias("StorageConfirm")]
    public Task<bool> ConfirmAsync(string token);
}

[StatelessWorker]
internal sealed class ConfirmGrain : Grain, IConfirmGrain
{
    public Task<bool> ConfirmAsync(string token)
    {
        // Implementation of confirmation logic goes here.
        // For example, check if the token exists in a database or cache.
        return Task.FromResult(true); // Placeholder implementation
    }
}
