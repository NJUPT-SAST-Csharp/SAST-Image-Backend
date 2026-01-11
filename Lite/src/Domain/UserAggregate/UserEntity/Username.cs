using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.UserAggregate.UserEntity;

[OpenJsonConverter<Username, string>]
public readonly record struct Username
    : IValueObject<Username, string>,
        IFactoryConstructor<Username, string>
{
    public const int MaxLength = 16;
    public const int MinLength = 2;

    public readonly string Value { get; }

    internal Username(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out Username newObject)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            newObject = default;
            return false;
        }

        input = input.Trim();

        if (input.Length > MaxLength || input.Length < MinLength)
        {
            newObject = default;
            return false;
        }

        newObject = new(input);
        return true;
    }
}
