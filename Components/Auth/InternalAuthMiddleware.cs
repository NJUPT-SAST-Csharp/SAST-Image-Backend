using System.Security.Claims;
using Identity;
using Microsoft.AspNetCore.Http;

namespace Auth;

internal sealed class InternalAuthMiddleware(RequestDelegate next)
{
    // TODO: Add logging
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (
            context.Request.Headers.TryGetValue(nameof(UserId), out var idValues) is false
            || context.Request.Headers.TryGetValue(nameof(Role), out var roleValues) is false
            || roleValues.Count != 1
            || idValues.Count != 1
            || long.TryParse(idValues[0], out long id) is false
            || Enum.TryParse<Role>(roleValues[0], out var role) is false
        )
        {
            ProxyNotAuthenticated(context);
            return;
        }

        if (
            endpoint is not null
            && endpoint.Metadata.GetOrderedMetadata<IRolesData>() is { Count: > 0 } datas
        )
        {
            var roles =
                datas.Count == 1 ? datas[0].Roles : [.. datas.SelectMany(data => data.Roles)];

            bool satisfy = false;

            for (int index = 0; index < roles.Length; index++)
            {
                var roleToCheck = roles[index];
                if ((role & roleToCheck) == roleToCheck)
                {
                    satisfy = true;
                    break;
                }
            }

            if (satisfy is false)
            {
                Forbid(context);
                return;
            }
        }

        ClaimsIdentity identity = new(
            [new Claim(nameof(UserId), idValues[0]!), new Claim(nameof(Role), roleValues[0]!)]
        );
        context.User = new(identity);

        await next(context);
    }

    private static void ProxyNotAuthenticated(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
        context.Response.ContentType = "application/json";
    }

    private static void Forbid(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
    }
}
