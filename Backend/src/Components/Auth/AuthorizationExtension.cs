using Identity;
using Microsoft.AspNetCore.Builder;

namespace Auth;

public static class AuthorizationExtension
{
    /// <summary>
    /// Registers the internal authentication middleware in the ASP.NET Core application pipeline.
    /// </summary>
    public static IApplicationBuilder UseInternalAuth(this IApplicationBuilder app)
    {
        app.UseMiddleware<InternalAuthMiddleware>();

        return app;
    }

    /// <summary>
    /// Adds authorization metadata to the endpoint builder, specifying the roles that are allowed to access the endpoint.
    /// </summary>
    /// <param name="allowedRoles">
    ///     <inheritdoc cref="RoleAttribute(Role[])"/>
    /// </param>
    public static RouteHandlerBuilder AddAuthorization(
        this RouteHandlerBuilder builder,
        params Role[] allowedRoles
    )
    {
        builder.Add(endpoint => endpoint.Metadata.Add(new RoleAttribute(allowedRoles)));
        return builder;
    }
}
