using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Primitives.Rule
{
    public sealed class DomainObjectValidationExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            if (exception is DomainObjectValidationException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.WriteAsJsonAsync<ProblemDetails>(
                    new()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                        Title = "Bad Request",
                        Detail = ex.Message
                    },
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            return ValueTask.FromResult(false);
        }
    }
}
