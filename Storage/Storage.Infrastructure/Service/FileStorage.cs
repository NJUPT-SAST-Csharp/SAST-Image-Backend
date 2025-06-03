using Minio;
using Minio.DataModel.Args;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Infrastructure.Service;

internal sealed class FileStorage(IMinioClient client) : IFileStorage
{
    public async Task<FileToken> AddAsync(
        IImageFile file,
        string bucketName,
        CancellationToken cancellationToken = default
    )
    {
        var token = FileToken.Create();

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
            .WithObject(token.Value)
            .WithObjectSize(file.Length)
            .WithBucket(bucketName)
            .WithContentType("image/*")
            .WithStreamData(file.Stream);

        await client.PutObjectAsync(args, cancellationToken);

        return token;
    }
}
