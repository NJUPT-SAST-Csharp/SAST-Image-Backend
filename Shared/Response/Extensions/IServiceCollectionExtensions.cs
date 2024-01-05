using Microsoft.Extensions.DependencyInjection;
using Response.ExceptionHandlers;

namespace Response.Extensions
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

        public static IServiceCollection AddDbNotFoundExceptionHandler(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<DbNotFoundExceptionHandler>();
            return services;
        }
    }
}
