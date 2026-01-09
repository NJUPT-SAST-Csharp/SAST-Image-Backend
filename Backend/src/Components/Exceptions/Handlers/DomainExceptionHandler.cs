using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Primitives.Exceptions;

namespace Exceptions.Handlers;

internal sealed class DomainExceptionHandler(
    IProblemDetailsService service,
    DomainExceptionHandlerContainer container
) : IExceptionHandler
{
    private readonly ProblemDetails defaultProblemDetails = new()
    {
        Status = StatusCodes.Status418ImATeapot,
        Title = "Unknown domain exception.",
        Detail = "An unknown domain exception occurred.",
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.19",
    };

    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not DomainException ex)
            return ValueTask.FromResult(false);

        if (container.Handlers.TryGetValue(ex.GetType(), out var handler) is false)
        {
            return service.TryWriteAsync(
                new()
                {
                    HttpContext = httpContext,
                    ProblemDetails = defaultProblemDetails,
                    Exception = exception,
                }
            );
        }

        var problemDetail = handler(ex);
        return service.TryWriteAsync(
            new()
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetail,
                Exception = exception,
            }
        );
    }
}
