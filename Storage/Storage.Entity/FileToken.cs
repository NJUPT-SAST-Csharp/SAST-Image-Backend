using System.Security.Cryptography;

namespace Storage.Entity;

public readonly record struct FileToken(string Token, string ObjectName)
{
    public static FileToken Create(string objectName) =>
        new(RandomNumberGenerator.GetHexString(64), objectName);

    public static implicit operator KeyValuePair<string, string>(FileToken token) =>
        new(token.Token, token.ObjectName);
}
