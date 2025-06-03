using Storage.Application.Model;

namespace Storage.Application.Service;

public interface IFileStorage
{
    public Task<FileToken> AddAsync(
        IImageFile file,
        string bucketName,
        CancellationToken cancellationToken = default
    );
}
