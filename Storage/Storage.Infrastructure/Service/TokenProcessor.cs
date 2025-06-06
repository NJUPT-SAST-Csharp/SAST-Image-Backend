using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Storage.Application.Model;
using Storage.Application.Service;
using Storage.Infrastructure.Models;

namespace Storage.Infrastructure.Service;

internal sealed class TokenProcessor(IOptions<StorageConfiguration> options)
    : ITokenIssuer,
        ITokenValidator
{
    private readonly byte[] seckey = Encoding.UTF8.GetBytes(options.Value.SecretKey);
    private const int BufferLength = 120; // 64 + 16 + 8 + 32

    public bool TryCreateNew(string bucketName, [NotNullWhen(true)] out IFileToken? token)
    {
        return TryCreateNew(bucketName, TimeSpan.MaxValue, out token);
    }

    public bool TryCreateNew(
        string bucketName,
        TimeSpan expireTime,
        [NotNullWhen(true)] out IFileToken? token
    )
    {
        // bucket: <= 64 bytes, object: 16 bytes, expireTimestamp: 8 bytes, hash: 32 bytes
        Span<byte> bytes = stackalloc byte[BufferLength];

        long timestamp;
        try
        {
            timestamp = DateTime.UtcNow.Add(expireTime).ToBinary();
        }
        catch (ArgumentOutOfRangeException)
        {
            timestamp = DateTime.MaxValue.ToBinary();
        }

        token = null;
        int offset = 0;

        // bucket: <= 64 bytes
        if (
            Encoding.ASCII.TryGetBytes(
                bucketName.ToLowerInvariant(),
                bytes[offset..64],
                out int bucketLength
            )
            is false
        )
            return false;

        offset += bucketLength;

        // object: 16 bytes (GUID)
        if (
            Guid.CreateVersion7()
                .TryWriteBytes(
                    bytes[offset..(offset + Unsafe.SizeOf<Guid>())],
                    true,
                    out int objectLength
                )
            is false
        )
            return false;

        offset += objectLength;

        // expireTimestamp: 8 bytes (long)
        if (BitConverter.TryWriteBytes(bytes[offset..(offset + sizeof(long))], timestamp) is false)
            return false;

        offset += sizeof(long);

        if (
            HMACSHA256.TryHashData(
                seckey,
                bytes[..offset],
                bytes[offset..(offset + HMACSHA256.HashSizeInBytes)],
                out int hashLength
            )
            is false
        )
            return false;

        offset += hashLength;

        bytes = bytes[..offset];

        token = new FileToken(Base64Url.EncodeToString(bytes));
        return true;
    }

    public bool TryValidate(string? value, [NotNullWhen(true)] out IFileToken? token)
    {
        Span<byte> buffer = stackalloc byte[BufferLength];
        Span<byte> compare = stackalloc byte[HMACSHA256.HashSizeInBytes];

        token = null;
        int offset;

        if (Base64Url.TryDecodeFromChars(value, buffer, out int bufferLength) is false)
            return false;

        offset = bufferLength;

        if (
            HMACSHA256.TryHashData(
                seckey,
                buffer[..(offset - HMACSHA256.HashSizeInBytes)],
                compare,
                out int hashLength
            )
            is false
        )
            return false;

        offset -= hashLength;

        if (compare.SequenceEqual(buffer[offset..bufferLength]) is false)
            return false;

        long timestamp = BitConverter.ToInt64(buffer[(offset - 8)..offset]);
        if (DateTime.FromBinary(timestamp) < DateTime.UtcNow)
            return false;

        token = new FileToken(value!);
        return true;
    }
}
