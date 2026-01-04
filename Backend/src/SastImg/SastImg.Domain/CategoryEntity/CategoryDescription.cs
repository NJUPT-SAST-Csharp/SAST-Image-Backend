using System.Diagnostics.CodeAnalysis;
using Primitives.Utilities;
using Primitives.ValueObject;

namespace SastImg.Domain.CategoryEntity;

[OpenJsonConverter<CategoryDescription, string>]
public readonly record struct CategoryDescription : IValueObject<CategoryDescription, string>
{
    public const int MaxLength = 100;

    public string Value { get; init; }

    public static bool TryCreateNew(
        string? input,
        [NotNullWhen(true)] out CategoryDescription newObject
    )
    {
        if (input is { Length: > MaxLength })
        {
            newObject = default;
            return false;
        }

        newObject = new() { Value = input?.Trim() ?? string.Empty };
        return true;
    }
}
