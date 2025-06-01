namespace Primitives.Exceptions;

public class ValueObjectInvalidException : DomainException
{
    public Type Type { get; init; } = default!;
    public object? ValueObject { get; init; }
}
