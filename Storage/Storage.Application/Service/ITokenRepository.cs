using Storage.Application.Model;

namespace Storage.Application.Service;

public interface ITokenRepository
{
    public Task<bool> ConfirmAsync(FileToken token, CancellationToken cancellationToken = default);

    public Task InsertAsync(FileToken token, CancellationToken cancellationToken = default);
}
