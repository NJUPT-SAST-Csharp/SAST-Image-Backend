using System.Reflection;
using Account.WebAPI.SeedWorks;
using Identity;
using Response.Extensions;

namespace Account.WebAPI.Configurations;

internal static class EndpointExtentions
{
    public static RouteHandlerBuilder AddAuthorization(
        this RouteHandlerBuilder builder,
        params Roles[] roles
    )
    {
        builder.RequireAuthorization(Array.ConvertAll(roles, r => r.ToString()));

        builder.WithUnauthorizedResponse();

        return builder;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var builder = app.MapGroup("/api");
        if (app.Environment.IsDevelopment())
        {
            builder.WithOpenApi();
        }

        var mappers = app.Services.GetServices<IEndpointMapper>();

        foreach (var mapper in mappers)
        {
            mapper.MapEndpoints(builder);
        }

        return app;
    }

    public static IServiceCollection RegisterEndpointMappersFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var types = assembly.GetTypes();

        var array = Array.FindAll(
            types,
            t => typeof(IEndpointMapper).IsAssignableFrom(t) && t.IsClass
        );

        foreach (var t in array)
        {
            services.AddTransient(typeof(IEndpointMapper), t);
        }
        return services;
    }

    public static RouteHandlerBuilder AddValidator<TRequest>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<ValidationFilter<TRequest>>();
        return builder;
    }
}
