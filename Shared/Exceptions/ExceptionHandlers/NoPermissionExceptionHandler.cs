using Exceptions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exceptions.ExceptionHandlers
{
    public sealed class NoPermissionExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            if (exception is NoPermissionException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                httpContext.Response.WriteAsJsonAsync(
                    new ProblemDetails()
                    {
                        Detail = exception.Message,
                        Title = "No Permission",
                        Status = StatusCodes.Status403Forbidden
                    },
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            return ValueTask.FromResult(false);
        }
    }
}
