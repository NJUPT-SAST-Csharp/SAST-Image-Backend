using System.Diagnostics.CodeAnalysis;
using Primitives.ValueObject;

namespace Account.Domain.UserEntity.ValueObjects;

public readonly record struct ImageToken(string Value) : IValueObject<ImageToken, string>
{
    public static bool TryCreateNew(string input, [NotNullWhen(true)] out ImageToken newObject)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > 128)
        {
            newObject = default;
            return false;
        }
        newObject = new(input);
        return true;
    }
}
