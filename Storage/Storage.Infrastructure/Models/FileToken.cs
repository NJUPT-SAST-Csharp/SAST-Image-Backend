using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

public sealed class FileToken : IFileToken, IEquatable<FileToken>
{
    private readonly byte[] bytes;
    private const int BufferLength = 120; // 64 + 16 + 8 + 32

    public const int MaxBucketNameLength = 64;

    private FileToken(string base64String) => bytes = Base64Url.DecodeFromChars(base64String);

    public string Value => Base64Url.EncodeToString(bytes);
    public DateTime ExpireAt => DateTime.FromBinary(BitConverter.ToInt64(bytes.AsSpan()[^40..^32]));
    public string ObjectName => new Guid(bytes.AsSpan()[^56..^40]).ToString();
    public string BucketName => Encoding.ASCII.GetString(bytes.AsSpan()[..^56]).TrimEnd('\0');

    public static bool TryValidate(
        string? value,
        ReadOnlySpan<byte> seckey,
        [NotNullWhen(true)] out FileToken? token
    )
    {
        Span<byte> buffer = stackalloc byte[BufferLength];
        Span<byte> compare = stackalloc byte[HMACSHA256.HashSizeInBytes];

        token = null;
        int offset;

        if (
            Base64Url.TryDecodeFromChars(value, buffer, out int bufferLength) is false
            || bufferLength <= 56
        )
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

        token = new(value!);
        return true;
    }

    public static bool TryCreateNew(
        string bucketName,
        ReadOnlySpan<byte> seckey,
        [NotNullWhen(true)] out FileToken? token
    )
    {
        return TryCreateNew(bucketName, TimeSpan.MaxValue, seckey, out token);
    }

    public static bool TryCreateNew(
        string bucketName,
        TimeSpan expireTime,
        ReadOnlySpan<byte> seckey,
        [NotNullWhen(true)] out FileToken? token
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

    public override string ToString() => Value;

    public bool Equals(IFileToken? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value == other.Value;
    }

    public bool Equals(FileToken? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as FileToken);

    public override int GetHashCode() => Value.GetHashCode();
}
