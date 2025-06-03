using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Storage.Application.Model;

public readonly record struct FileToken
{
    public string Value { get; private init; }

    public static bool TryParse(string? value, [NotNullWhen(true)] out FileToken? token)
    {
        if (string.IsNullOrEmpty(value) || value.Length != 64 || !value.IsUppercaseHex())
        {
            token = default;
            return false;
        }
        token = new FileToken() { Value = value };
        return true;
    }

    public static FileToken Create() => new() { Value = RandomNumberGenerator.GetHexString(64) };
}

file static class StringExtensions
{
    public static bool IsUppercaseHex(this string value)
    {
        foreach (char c in value)
        {
            if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F')))
            {
                return false;
            }
        }
        return true;
    }
}
