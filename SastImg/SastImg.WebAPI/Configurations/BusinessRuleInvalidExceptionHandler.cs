using Microsoft.AspNetCore.Diagnostics;
using Primitives.Rule;
using Response.ReponseObjects;

namespace SastImg.WebAPI.Configurations
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public class BusinessRuleInvalidExceptionHandler : IExceptionHandler
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
                httpContext.Response.WriteAsJsonAsync<BadRequestResponse>(
                    new(ruleInvalidException.Message, ruleInvalidException.Details),
                    cancellationToken
                );
                return ValueTask.FromResult(true);
            }
            return ValueTask.FromResult(false);
        }
    }
}
