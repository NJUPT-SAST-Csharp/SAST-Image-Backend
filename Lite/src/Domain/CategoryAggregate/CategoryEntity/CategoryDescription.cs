using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.CategoryAggregate.CategoryEntity;

public readonly record struct CategoryDescription
    : IValueObject<CategoryDescription, string>,
        IFactoryConstructor<CategoryDescription, string>
{
    public const int MaxLength = 100;
    public string Value { get; }

    internal CategoryDescription(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(
        string input,
        [NotNullWhen(true)] out CategoryDescription newObject
    )
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            newObject = default;
            return false;
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
