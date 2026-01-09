using System.Diagnostics.CodeAnalysis;

namespace Primitives.Exceptions;

public abstract class DomainException : Exception
{
    public static void ThrowIf<T>([DoesNotReturnIf(true)] bool condition)
        where T : DomainException, new()
    {
        if (condition)
        {
            throw new T();
        }
    }
}

public abstract class DomainException<T> : DomainException
    where T : DomainException, new()
{
    public static void ThrowIf([DoesNotReturnIf(true)] bool condition)
    {
        if (condition)
        {
            throw new T();
        }
    }
}
