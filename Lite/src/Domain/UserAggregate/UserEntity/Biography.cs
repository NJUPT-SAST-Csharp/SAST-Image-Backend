using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.UserAggregate.UserEntity;

[OpenJsonConverter<Biography, string>]
public readonly record struct Biography
    : IValueObject<Biography, string>,
        IFactoryConstructor<Biography, string>
{
    public static readonly Biography Empty = new(string.Empty);

    public const int MaxLength = 50;

    public string Value { get; }

    internal Biography(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out Biography newObject)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            newObject = Empty;
            return true;
        }

        input = input.Trim();

        if (input.Length > MaxLength)
        {
            newObject = default;
            return false;
        }

        newObject = new(input);
        return true;
    }
}
