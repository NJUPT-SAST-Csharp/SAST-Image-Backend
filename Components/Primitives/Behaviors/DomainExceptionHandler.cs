using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Primitives.Exceptions;

namespace Primitives.Behaviors;

internal sealed class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not DomainException domainEx)
            return false;

        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        ProblemDetails response = domainEx switch
        {
            ValueObjectInvalidException and { Rule: { } rule } => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = rule.ParameterName is null
                    ? $"The value [{rule.ActualValue}] is invalid."
                    : $"The value of [{rule.ParameterName}]: [{rule.ActualValue}] is invalid.",
                Title = "Validation failed.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            },

            _ => new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Unknown domain exception.",
                Detail = exception.Message,
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                Extensions = new Dictionary<string, object?>
                {
                    ["exceptionType"] = exception.GetType().FullName,
                },
            },
        };

        response.Extensions.Add("traceId", traceId);
        httpContext.Response.StatusCode = response.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
