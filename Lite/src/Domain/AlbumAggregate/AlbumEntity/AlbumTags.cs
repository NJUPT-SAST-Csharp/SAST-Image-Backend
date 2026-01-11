using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.AlbumAggregate.AlbumEntity;

[OpenJsonConverter<AlbumTags, string[]>]
public readonly struct AlbumTags
    : IValueObject<AlbumTags, string[]>,
        IFactoryConstructor<AlbumTags, string[]>
{
    public const int MaxCount = 10;
    public const int MaxLength = 12;

    internal AlbumTags(string[] array) => Value = array;

    public string[] Value { get; } = [];

    public static bool TryCreateNew(string[] input, [NotNullWhen(true)] out AlbumTags entity)
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

    public bool Equals(AlbumTags other)
    {
        return Value.SequenceEqual(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is AlbumTags other && Equals(other);
    }

    public static bool operator ==(AlbumTags left, AlbumTags right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AlbumTags left, AlbumTags right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public static implicit operator string[](AlbumTags AlbumTags) => AlbumTags.Value;

    public static readonly AlbumTags Empty = new([]);
}
