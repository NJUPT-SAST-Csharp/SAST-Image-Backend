using System.Diagnostics.CodeAnalysis;
using Primitives.Rule;
using Primitives.ValueObject;

namespace Primitives.Exceptions;

public sealed class ValueObjectInvalidException(string value, string? param = null)
    : DomainException(new DomainModelValid { ActualValue = value, ParameterName = param })
{
    [DoesNotReturn]
    public static void Throw<TObject, TValue>(IValueObject<TObject, TValue>? obj)
        where TObject : IValueObject<TObject, TValue> =>
        throw new ValueObjectInvalidException(obj?.Value?.ToString() ?? "{null}");

    public new DomainModelValid Rule => (DomainModelValid)base.Rule;

    public sealed class DomainModelValid() : IDomainRule
    {
        public required string ActualValue { get; init; }
        public string? ParameterName { get; init; }

        public bool IsBroken { get; } = true;
    }
}
