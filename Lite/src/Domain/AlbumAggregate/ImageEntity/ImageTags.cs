using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.AlbumAggregate.ImageEntity;

[OpenJsonConverter<ImageTags, string[]>]
public readonly struct ImageTags
    : IValueObject<ImageTags, string[]>,
        IFactoryConstructor<ImageTags, string[]>
{
    public const int MaxCount = 10;
    public const int MaxLength = 12;

    internal ImageTags(string[] array) => Value = array;

    public string[] Value { get; } = [];

    public static bool TryCreateNew(string[] input, [NotNullWhen(true)] out ImageTags entity)
    {
        if (input.Length == 0)
        {
            entity = Empty;
            return true;
        }

        string[] mid = [.. input.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()];

        if (mid.Length > MaxCount)
        {
            entity = default;
            return false;
        }

        if (mid.Any(x => x.Length > MaxLength))
        {
            entity = default;
            return false;
        }

        entity = new(mid);
        return true;
    }

    public bool Equals(ImageTags other)
    {
        return Value.SequenceEqual(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is ImageTags other && Equals(other);
    }

    public static bool operator ==(ImageTags left, ImageTags right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ImageTags left, ImageTags right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public static implicit operator string[](ImageTags imageTags) => imageTags.Value;

    public static readonly ImageTags Empty = new([]);
}
