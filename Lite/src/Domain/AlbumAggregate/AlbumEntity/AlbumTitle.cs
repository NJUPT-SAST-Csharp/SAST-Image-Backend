using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.AlbumAggregate.AlbumEntity;

[OpenJsonConverter<AlbumTitle, string>]
public readonly record struct AlbumTitle
    : IValueObject<AlbumTitle, string>,
        IFactoryConstructor<AlbumTitle, string>
{
    public const int MaxLength = 20;
    public const int MinLength = 1;

    public string Value { get; }

    internal AlbumTitle(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(string value, [NotNullWhen(true)] out AlbumTitle newObject)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            newObject = default;
            return false;
        }

        value = value.Trim();

        if (value.Length > MaxLength || value.Length < MinLength)
        {
            newObject = default;
            return false;
        }

        newObject = new(value);
        return true;
    }
}
