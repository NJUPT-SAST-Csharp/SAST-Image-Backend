using System.Diagnostics.CodeAnalysis;
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
    private readonly byte[] seckey = Encoding.ASCII.GetBytes(options.Value.SecretKey);
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
        bool result = FileToken.TryCreateNew(bucketName, expireTime, seckey, out var filetoken);
        token = filetoken;
        return result;
    }

    public bool TryValidate(string? value, [NotNullWhen(true)] out IFileToken? token)
    {
        bool result = FileToken.TryValidate(value, seckey, out var filetoken);
        token = filetoken;
        return result;
    }
}
