namespace Domain.Entity;

public interface IValueObject<TObject, TValue> : IEquatable<TObject>
    where TObject : IValueObject<TObject, TValue>
{
    public TValue Value { get; }
}
