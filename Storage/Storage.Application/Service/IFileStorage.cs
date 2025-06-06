using System.IO.Pipelines;
using Storage.Application.Model;

namespace Storage.Application.Service;

public interface IFileStorage
{
    public Task<IFileToken?> AddAsync(
        IImageFile file,
        string bucketName,
        CancellationToken cancellationToken = default
    );

    public Task<IImageFile?> GetAsync(
        IFileToken token,
        CancellationToken cancellationToken = default
    );

    public Task<bool> TryWriteAsync(
        IFileToken token,
        PipeWriter writer,
        CancellationToken cancellationToken = default
    );

    public Task<bool> DeleteAsync(IFileToken token, CancellationToken cancellationToken = default);

    public Task<IFileToken[]> DeleteAsync(
        IFileToken[] tokens,
        CancellationToken cancellationToken = default
    );
}
