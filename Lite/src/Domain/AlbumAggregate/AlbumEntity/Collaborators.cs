using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.AlbumEntity;

[OpenJsonConverter<Collaborators, UserId[]>]
public readonly struct Collaborators
    : IValueObject<Collaborators, UserId[]>,
        IFactoryConstructor<Collaborators, UserId[]>
{
    public const int MaxCount = 32;

    public UserId[] Value { get; }

    internal Collaborators(UserId[] array) => Value = array;

    public static bool TryCreateNew(
        UserId[] value,
        [MaybeNullWhen(false), NotNullWhen(true)] out Collaborators newObject
    )
    {
        var mid = value.Distinct().ToArray();

        if (mid.Length > MaxCount)
        {
            newObject = default;
            return false;
        }

        newObject = new(mid);
        return true;
    }

    public bool Equals(Collaborators other)
    {
        return Value.SequenceEqual(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Collaborators collaborators && Equals(collaborators);
    }

    public static bool operator ==(Collaborators left, Collaborators right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Collaborators left, Collaborators right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public static readonly Collaborators Empty = new(Array.Empty<UserId>());
}
