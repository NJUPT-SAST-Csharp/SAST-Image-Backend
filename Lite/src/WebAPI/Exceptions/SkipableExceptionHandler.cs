using Microsoft.AspNetCore.Diagnostics;

namespace WebAPI.Exceptions;

public sealed class SkipableExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is TaskCanceledException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;

            return ValueTask.FromResult(true);
        }

        return ValueTask.FromResult(false);
    }
}
