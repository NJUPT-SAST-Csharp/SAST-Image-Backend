using System.Diagnostics.CodeAnalysis;
using Primitives.Utilities;
using Primitives.ValueObject;

namespace Account.Domain.UserEntity.ValueObjects;

[OpenJsonConverter<Username, string>]
public readonly record struct Username : IValueObject<Username, string>
{
    public const int MaxLength = 16;
    public const int MinLength = 2;

    public readonly string Value { get; init; }

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

        newObject = new() { Value = input };
        return true;
    }
}
