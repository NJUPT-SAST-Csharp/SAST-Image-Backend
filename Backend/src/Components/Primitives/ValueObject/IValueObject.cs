using System.Diagnostics.CodeAnalysis;

namespace Primitives.ValueObject;

public interface IValueObject<TObject, TValue> : IEquatable<TObject>
    where TObject : IValueObject<TObject, TValue>
{
    public TValue Value { get; init; }

    public static abstract bool TryCreateNew(
        TValue input,
        [NotNullWhen(true)] out TObject? newObject
    );

    public string ToString()
    {
        return Value?.ToString() ?? ToString();
    }
}
