using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared;
using Domain.Shared.Converter;

namespace Domain.UserAggregate.UserEntity;

public sealed record class Password
{
    private Password() { }

    internal Password(byte[] hash, byte[] salt)
    {
        Hash = hash;
        Salt = salt;
    }

    public byte[] Hash { get; } = null!;
    public byte[] Salt { get; } = null!;
}

[OpenJsonConverter<PasswordInput, string>]
public readonly record struct PasswordInput
    : IValueObject<PasswordInput, string>,
        IFactoryConstructor<PasswordInput, string>
{
    public const int MaxLength = 20;
    public const int MinLength = 6;

    public string Value { get; }

    internal PasswordInput(string value)
    {
        Value = value;
    }

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

        if (input.IsLetterOrDigitOrUnderline() == false)
        {
            newObject = default;
            return false;
        }

        newObject = new(input);
        return true;
    }
}
