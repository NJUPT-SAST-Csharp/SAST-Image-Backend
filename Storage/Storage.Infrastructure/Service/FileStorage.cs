using Minio;
using Minio.DataModel.Args;
using Storage.Application.Model;
using Storage.Application.Service;
using Storage.Infrastructure.Models;

namespace Storage.Infrastructure.Service;

internal sealed class FileStorage(IMinioClient client, ITokenIssuer factory) : IFileStorage
{
    public async Task<FileToken> AddAsync(
        IImageFile file,
        string bucketName,
        CancellationToken cancellationToken = default
    )
    {
        factory.TryCreateNew(bucketName, out var token);

        bool exists = await client.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName),
            cancellationToken
        );
        if (exists is false)
        {
            await client.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName),
                cancellationToken
            );
        }

        var args = new PutObjectArgs()
            .WithObject(token!.Value.ObjectName.ToString())
            .WithObjectSize(file.Length)
            .WithBucket(bucketName)
            .WithContentType($"image/{file.Format}")
            .WithStreamData(file.Stream);

        await client.PutObjectAsync(args, cancellationToken);

        return token.Value;
    }

    public async Task<IImageFile> GetImageAsync(
        FileToken token,
        CancellationToken cancellationToken = default
    )
    {
        IImageFile file = null!;

        var args = new GetObjectArgs()
            .WithObject(token.ObjectName.ToString())
            .WithBucket(token.BucketName)
            .WithCallbackStream(
                async (stream, cancellationToken) =>
                {
                    file = await ManagedImageFile.CreateAsync(stream, cancellationToken);
                }
            );

        await client.GetObjectAsync(args, cancellationToken);
        return file;
    }
}
