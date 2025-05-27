using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Primitives.ValueObject;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity;

[OpenJsonConverter<AlbumTitle, string>]
[TypeConverter(typeof(OpenType<AlbumTitle>))]
public readonly record struct AlbumTitle : IValueObject<AlbumTitle, string>
{
    public const int MinLength = 1;
    public const int MaxLength = 10;

    public readonly string Value { get; init; }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out AlbumTitle newObject)
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
