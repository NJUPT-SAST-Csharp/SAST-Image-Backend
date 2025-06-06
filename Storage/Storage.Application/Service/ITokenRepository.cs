using Storage.Application.Model;

namespace Storage.Application.Service;

public interface ITokenRepository
{
    public Task<bool> ExistsAsync(IFileToken token, CancellationToken cancellationToken = default);

    public Task AddAsync(IFileToken token, CancellationToken cancellationToken = default);

    public Task<IFileToken[]> GetExpiredAsync(CancellationToken cancellationToken = default);
    public Task DeleteAsync(IFileToken token, CancellationToken cancellationToken = default);
    public Task DeleteAsync(IFileToken[] tokens, CancellationToken cancellationToken = default);
}
