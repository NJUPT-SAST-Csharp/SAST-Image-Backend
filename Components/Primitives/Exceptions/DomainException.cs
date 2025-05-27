using System.Diagnostics.CodeAnalysis;
using Primitives.Rule;

namespace Primitives.Exceptions;

public class DomainException(IDomainRule rule) : Exception
{
    public IDomainRule Rule { get; } = rule;

    [DoesNotReturn]
    public static void Throw<TRule>(TRule rule)
        where TRule : IDomainRule => throw new DomainException(rule);
}
