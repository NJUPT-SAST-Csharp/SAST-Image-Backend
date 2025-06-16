using System.Diagnostics.CodeAnalysis;
using Primitives.Utilities;
using Primitives.ValueObject;

namespace Account.Domain.UserEntity.ValueObjects;

public sealed record class Password
{
    public required byte[] Hash { get; init; }
    public required byte[] Salt { get; init; }
}

[OpenJsonConverter<PasswordInput, string>]
public readonly record struct PasswordInput : IValueObject<PasswordInput, string>
{
    public const int MaxLength = 20;
    public const int MinLength = 6;

    public string Value { get; init; }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out PasswordInput newObject)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            newObject = default;
            return false;
        }

        input = input.Trim();

        if (input.Length < MinLength || input.Length > MaxLength)
        {
            newObject = default;
            return false;
        }

        if (IsLetterOrDigitOrUnderline(input) == false)
        {
            newObject = default;
            return false;
        }

        newObject = new() { Value = input };
        return true;
    }

    private static bool IsLetterOrDigitOrUnderline(string value)
    {
        foreach (char c in value)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
            {
                return false;
            }
        }

        return true;
    }
}
