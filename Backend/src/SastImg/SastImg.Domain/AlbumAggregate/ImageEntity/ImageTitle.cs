using System.Diagnostics.CodeAnalysis;
using Primitives.Utilities;
using Primitives.ValueObject;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

[OpenJsonConverter<ImageDescription, string>]
public readonly record struct ImageTitle : IValueObject<ImageTitle, string>
{
    public const int MinLength = 1;
    public const int MaxLength = 12;

    public readonly string Value { get; init; }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out ImageTitle newObject)
    {
        if (input is not { Length: >= MinLength and <= MaxLength })
        {
            newObject = default;
            return false;
        }

        newObject = new() { Value = input };
        return true;
    }
}
