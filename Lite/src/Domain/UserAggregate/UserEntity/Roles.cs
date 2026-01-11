using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.UserAggregate.UserEntity;

public sealed class Roles
    : ReadOnlyCollection<Role>,
        IValueObject<Roles, IReadOnlyCollection<Role>>,
        IFactoryConstructor<Roles, IEnumerable<Role>>
{
    public IReadOnlyCollection<Role> Value => [.. Items];

    internal Roles(IList<Role> list)
        : base(list) { }

    public Roles()
        : this([]) { }

    public bool Equals(Roles? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Items.SequenceEqual(other.Items);
    }

    public static bool TryCreateNew(
        IEnumerable<Role> input,
        [NotNullWhen(true)] out Roles? newObject
    )
    {
        if (input.Any(role => !Enum.IsDefined(role)))
        {
            newObject = null;
            return false;
        }

        newObject = new(input.ToList());
        return true;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Roles);
    }

    public override int GetHashCode()
    {
        return Items.GetHashCode();
    }
}
