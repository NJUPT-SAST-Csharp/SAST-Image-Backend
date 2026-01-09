using System.IO.Pipelines;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using Storage.Application.Model;
using Storage.Application.Service;
using Storage.Infrastructure.Models;

namespace Storage.Infrastructure.Service;

internal sealed class FileStorage(IMinioClient client, ITokenIssuer factory) : IFileStorage
{
    public async Task<IFileToken?> AddAsync(
        IImageFile file,
        string bucketName,
        CancellationToken cancellationToken = default
    )
    {
        const int expireMinutes = 10;

        try
        {
            if (
                factory.TryCreateNew(bucketName, TimeSpan.FromMinutes(expireMinutes), out var token)
                is false
            )
                return token;

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
                .WithObject(token!.ObjectName.ToString())
                .WithObjectSize(file.Length)
                .WithBucket(bucketName)
                .WithContentType($"image/{file.Format}")
                .WithStreamData(file.Stream);

            await client.PutObjectAsync(args, cancellationToken);

            return token;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(
        IFileToken token,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithObject(token.ObjectName.ToString())
                .WithBucket(token.BucketName);

            await client.RemoveObjectAsync(args, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IFileToken[]> DeleteAsync(
        IFileToken[] tokens,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            string bucketName = tokens[0].BucketName;

            var args = new RemoveObjectsArgs()
                .WithBucket(bucketName)
                .WithObjects([.. tokens.Select(t => t.ObjectName.ToString())]);

            var failList = await client.RemoveObjectsAsync(args, cancellationToken);
            if (failList.Count <= 0)
                return [];

            var failedKeys = failList.Select(f => f.Key);
            return Array.FindAll(tokens, t => failedKeys.Contains(t.ObjectName) is false);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> TryWriteAsync(
        IFileToken token,
        PipeWriter writer,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var args = new GetObjectArgs()
                .WithObject(token.ObjectName.ToString())
                .WithBucket(token.BucketName)
                .WithCallbackStream(
                    async (stream, cancellationToken) =>
                    {
                        while (true)
                        {
                            var memory = writer.GetMemory(4 * 1024);
                            int bytesRead = await stream.ReadAsync(memory, cancellationToken);
                            if (bytesRead <= 0)
                                break;
                            writer.Advance(bytesRead);
                            var result = await writer.FlushAsync(cancellationToken);
                            if (result.IsCompleted)
                                break;
                        }
                    }
                );
            await client.GetObjectAsync(args, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
