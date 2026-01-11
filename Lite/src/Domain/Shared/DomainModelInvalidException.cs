using Domain.Extensions;

namespace Domain.Shared;

public sealed class DomainModelInvalidException(string? value) : DomainException
{
    public string? InputValue { get; } = value;
    public string? ParamName { get; } = null;

    internal DomainModelInvalidException(string? value, string? paramName)
        : this(value)
    {
        ParamName = paramName;
    }
}
