using Exceptions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exceptions.ExceptionHandlers
{
    public sealed class DbNotFoundExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            if (exception is DbNotFoundException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                httpContext.Response.WriteAsJsonAsync(
                    new ProblemDetails()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Detail = ex.Message,
                        Title = "Not Found",
                    },
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            else if (
                exception is InvalidOperationException invalidEx
                && invalidEx.Message == "Sequence contains no elements"
            )
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                httpContext.Response.WriteAsJsonAsync(
                    new ProblemDetails()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Not Found",
                        Detail = exception.Message,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    },
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }
    }
}
