using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Response.ReponseObjects;

namespace Response.ExceptionHandlers
{
    public sealed class DefaultExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            httpContext.Response.WriteAsJsonAsync(
                new BadRequestResponse("Bad Request", exception.Message),
                cancellationToken
            );
            return ValueTask.FromResult(true);
        }
    }
}
