using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Primitives.Configuration;
using Primitives.Extensions;

namespace Primitives
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInternalEventBus(
            this IServiceCollection services,
            Action<EventBusOptions>? optionsBuilder = null
        )
        {
            EventBusOptions options = new();

            optionsBuilder ??= options => options.AddDefaultBuses();

            optionsBuilder(options);

            if (options.Assemblies.Length == 0)
            {
                throw new InvalidOperationException("At least one assembly must be provided.");
            }

            services.AddMediatR(option =>
            {
                option.AutoRegisterRequestProcessors = true;
                option.RegisterServicesFromAssemblies(options.Assemblies);
            });
            services.TryAddEnumerable(options.Services);

            return services;
        }

        public static IServiceCollection AddInternalUnitOfWork<TDbContext>(
            this IServiceCollection services
        )
            where TDbContext : DbContext
        {
            services.TryAddScoped<IUnitOfWork, DefaultUnitOfWork<TDbContext>>();

            return services;
        }

        public static IServiceCollection AddInternalUnitOfWork<TWriteDbContext, TQueryDbContext>(
            this IServiceCollection services
        )
            where TWriteDbContext : DbContext
            where TQueryDbContext : DbContext
        {
            services.TryAddScoped<
                IUnitOfWork,
                DefaultUnitOfWork<TWriteDbContext, TQueryDbContext>
            >();

            return services;
        }
    }
}
