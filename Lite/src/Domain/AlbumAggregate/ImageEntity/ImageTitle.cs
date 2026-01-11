using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumAggregate.ImageEntity;

public readonly record struct ImageTitle
    : IValueObject<ImageTitle, string>,
        IFactoryConstructor<ImageTitle, string>
{
    public const int MaxLength = 20;

    public static readonly ImageTitle Empty = new(string.Empty);

    public string Value { get; }

    internal ImageTitle(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(string value, [NotNullWhen(true)] out ImageTitle newObject)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            newObject = Empty;
            return true;
        }

        value = value.Trim();

        if (value.Length > MaxLength)
        {
            newObject = default;
            return false;
        }

        newObject = new(value);
        return true;
    }
}
