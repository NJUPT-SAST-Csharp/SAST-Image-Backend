using Exceptions.ExceptionHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Exceptions.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultExceptionHandler(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<DefaultExceptionHandler>();
            return services;
        }
    }
}
