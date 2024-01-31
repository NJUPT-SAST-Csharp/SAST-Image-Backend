using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exceptions.ExceptionHandlers
{
    public sealed class DefaultExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message,
                    Title = "Unhandled Unknown Exception",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1"
                },
                cancellationToken
            );
            return ValueTask.FromResult(true);
        }
    }
}
