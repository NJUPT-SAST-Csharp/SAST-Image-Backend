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
                httpContext.Response.WriteAsJsonAsync<BadRequestObjectResult>(
                    new(new { ruleInvalidException.Message, ruleInvalidException.Details }),
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            return ValueTask.FromResult(false);
        }
    }
}
