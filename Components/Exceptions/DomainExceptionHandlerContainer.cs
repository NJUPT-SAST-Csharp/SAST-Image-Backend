using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Primitives.Exceptions;

namespace Exceptions;

public sealed class DomainExceptionHandlerContainer
{
    private readonly Dictionary<Type, Func<DomainException, ProblemDetails>> container = new()
    {
        [typeof(ValueObjectInvalidException)] = ex => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid Value Object",
            Detail = ex.Message,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
        },
        [typeof(EntityNotFoundException)] = ex => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Entity Not Found",
            Detail = ex.Message,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
        },
    };

    internal IReadOnlyDictionary<Type, Func<DomainException, ProblemDetails>> Handlers => container;

    public DomainExceptionHandlerContainer Register<TException>(
        Func<TException, ProblemDetails> handler
    )
        where TException : DomainException
    {
        container[typeof(TException)] = rule => handler((TException)rule);

        return this;
    }
}
