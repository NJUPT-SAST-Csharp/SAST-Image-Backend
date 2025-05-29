using Exceptions.ExceptionHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Exceptions.Configurations;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<DefaultExceptionHandler>();
        return services;
    }
}
