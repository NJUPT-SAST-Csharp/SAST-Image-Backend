using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Response.Exceptions;
using Response.ReponseObjects;

namespace Response.ExceptionHandlers
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
                httpContext.Response.WriteAsJsonAsync(
                    new BadRequestResponse(ex.Message, ex.Data.Values.Cast<string>().First()),
                    cancellationToken: cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            else if (
                exception is InvalidOperationException invalidEx
                && invalidEx.Message == "Sequence contains no elements"
            )
            {
                httpContext.Response.WriteAsJsonAsync(
                    new BadRequestResponse(invalidEx.Message),
                    cancellationToken: cancellationToken
                );
                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }
    }
}
