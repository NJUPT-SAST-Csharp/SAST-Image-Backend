using Minio;
using Minio.DataModel.Args;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Infrastructure.Service;

internal sealed class FileStorage(IMinioClient client) : IFileStorage
{
    public async Task<FileToken> AddAsync(
        IImageFile file,
        CancellationToken cancellationToken = default
    )
    {
        var args = new PutObjectArgs().WithStreamData(file.Stream);

        var response = await client.PutObjectAsync(args, cancellationToken);

        return FileToken.Create();
    }
}
