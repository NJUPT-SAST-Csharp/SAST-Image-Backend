using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Primitives.Rule;

namespace SastImg.WebAPI.Configurations
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public class DomainBusinessRuleInvalidExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            if (exception is DomainBusinessRuleInvalidException ruleInvalidException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.WriteAsJsonAsync<ProblemDetails>(
                    new()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                        Title = "Bad Request",
                        Detail = ruleInvalidException.Message
                    },
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            return ValueTask.FromResult(false);
        }
    }
}
