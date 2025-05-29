using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Exceptions.ExceptionHandlers;

public sealed class DefaultExceptionHandler(IProblemDetailsService factory) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return factory.TryWriteAsync(
            new()
            {
                HttpContext = httpContext,
                AdditionalMetadata = httpContext.GetEndpoint()?.Metadata,
                ProblemDetails = new()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message,
                    Title = "Unhandled Unknown Exception",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                },
            }
        );
    }
}
