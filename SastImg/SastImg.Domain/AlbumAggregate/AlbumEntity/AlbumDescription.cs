using System.Diagnostics.CodeAnalysis;
using Primitives.ValueObject;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity;

[OpenJsonConverter<AlbumDescription, string>]
public readonly record struct AlbumDescription : IValueObject<AlbumDescription, string>
{
    public const int MinLength = 2;
    public const int MaxLength = 100;

    public readonly string Value { get; init; }

    public static bool TryCreateNew(
        string input,
        [NotNullWhen(true)] out AlbumDescription newObject
    )
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
