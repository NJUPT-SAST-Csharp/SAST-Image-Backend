using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Exceptions;

public sealed class DefaultExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Title = "Unhandled Unknown Exception",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                Extensions = { ["traceId"] = traceId },
            },
            cancellationToken
        );
        return true;
    }
}
