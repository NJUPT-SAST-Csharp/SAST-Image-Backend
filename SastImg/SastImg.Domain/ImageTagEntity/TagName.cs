using System.Diagnostics.CodeAnalysis;
using Primitives.ValueObject;

namespace SastImg.Domain.AlbumTagEntity;

public readonly record struct TagName : IValueObject<TagName, string>
{
    public const int MaxLength = 12;
    public const int MinLength = 1;

    public string Value { get; init; }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out TagName newObject)
    {
        if (input is not { Length: >= MinLength and <= MaxLength })
        {
            newObject = default;
            return false;
        }

        newObject = new() { Value = input.Trim() };
        return true;
    }
}
