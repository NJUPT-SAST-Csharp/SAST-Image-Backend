using Exceptions.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Exceptions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultExceptionHandler(
        this IServiceCollection services,
        Action<DomainExceptionHandlerContainer>? containerBuilder = null
    )
    {
        DomainExceptionHandlerContainer container = new();
        containerBuilder?.Invoke(container);
        services.AddSingleton(container);

        services.AddProblemDetails();
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<DefaultExceptionHandler>();
        return services;
    }
}
