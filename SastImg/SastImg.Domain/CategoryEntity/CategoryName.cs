using System.Diagnostics.CodeAnalysis;
using Primitives.ValueObject;

namespace SastImg.Domain.CategoryEntity;

[OpenJsonConverter<CategoryName, string>]
public readonly record struct CategoryName : IValueObject<CategoryName, string>
{
    public const int MinLength = 1;

    public const int MaxLength = 20;

    public string Value { get; init; }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out CategoryName newObject)
    {
        if (input is not { Length: <= MaxLength and >= MinLength })
        {
            newObject = default;
            return false;
        }

        newObject = new() { Value = input.Trim() };
        return true;
    }
}
