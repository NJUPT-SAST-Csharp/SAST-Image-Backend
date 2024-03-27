using System.Diagnostics.CodeAnalysis;

namespace Primitives.Rule
{
    public sealed class DomainObjectValidationException(string message) : Exception(message)
    {
        [DoesNotReturn]
        public static void Throw<T>(T obj) =>
            throw new DomainObjectValidationException($"Invalid {typeof(T).Name} model: {obj}");
    }
}
