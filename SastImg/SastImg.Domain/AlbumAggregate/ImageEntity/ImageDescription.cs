using System.Diagnostics.CodeAnalysis;
using Primitives.ValueObject;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

[OpenJsonConverter<ImageDescription, string>]
public readonly record struct ImageDescription : IValueObject<ImageDescription, string>
{
    public const int MaxLength = 100;

    public readonly string Value { get; init; }

    public static bool TryCreateNew(
        string? input,
        [NotNullWhen(true)] out ImageDescription newObject
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
