using System.Diagnostics.CodeAnalysis;
using Storage.Application.Model;

namespace Storage.Application.Service;

public interface ITokenIssuer
{
    public bool TryCreateNew(string bucketName, [NotNullWhen(true)] out IFileToken? token);

    public bool TryCreateNew(
        string bucketName,
        TimeSpan expireTime,
        [NotNullWhen(true)] out IFileToken? token
    );
}
