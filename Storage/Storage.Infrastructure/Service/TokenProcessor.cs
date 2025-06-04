using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Infrastructure.Service;

internal sealed class TokenProcessor(IOptions<StorageConfiguration> options)
    : ITokenIssuer,
        ITokenValidator
{
    private readonly byte[] seckey = Encoding.UTF8.GetBytes(options.Value.SecretKey);

    public bool TryCreateNew(string bucketName, [NotNullWhen(true)] out FileToken? token)
    {
        if (bucketName.Length > 16)
        {
            token = null;
            return false;
        }

        Span<byte> bytes = stackalloc byte[FileToken.ByteLength];

        if (
            Encoding.ASCII.TryGetBytes(bucketName, bytes[..16], out int written) is false
            || Guid.CreateVersion7().TryWriteBytes(bytes[16..], true, out _) is false
            || HMACSHA256.TryHashData(
                seckey,
                bytes[..^HMACSHA256.HashSizeInBytes],
                bytes[^HMACSHA256.HashSizeInBytes..],
                out _
            )
                is false
        )
        {
            token = null;
            return false;
        }

        token = Create(bytes.ToArray());
        return true;
    }

    public bool TryValidate(string? value, [NotNullWhen(true)] out FileToken? token)
    {
        Span<byte> buffer = stackalloc byte[FileToken.ByteLength];
        Span<byte> compare = stackalloc byte[HMACSHA256.HashSizeInBytes];

        if (
            Base64Url.TryDecodeFromChars(value, buffer, out int bytesWritten) is false
            || bytesWritten != FileToken.ByteLength
            || HMACSHA256.TryHashData(
                seckey,
                buffer[..^HMACSHA256.HashSizeInBytes],
                compare,
                out bytesWritten
            )
                is false
            || compare.SequenceEqual(buffer[^HMACSHA256.HashSizeInBytes..]) is false
        )
        {
            token = null;
            return false;
        }

        token = Create(buffer[..bytesWritten].ToArray());
        return true;
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    static extern FileToken Create(byte[] value);
}
