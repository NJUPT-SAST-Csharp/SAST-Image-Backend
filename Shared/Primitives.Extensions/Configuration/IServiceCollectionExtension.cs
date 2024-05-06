using Microsoft.Extensions.DependencyInjection;
using Primitives.Extensions.Behaviors;
using Primitives.Extensions.Configuration;

namespace Primitives
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPrimitives(
            this IServiceCollection services,
            Action<PrimitivesOptions>? optionsBuilder = null
        )
        {
            PrimitivesOptions options = new(services);

            optionsBuilder?.Invoke(options);
            options.AddDefaultBuses();

            services.AddMediatR(option =>
            {
                option.AutoRegisterRequestProcessors = true;
                option.RegisterServicesFromAssemblies(options.Assemblies);
                if (options.AutoCommitAfterCommandHandled)
                {
                    option.AddOpenRequestPostProcessor(typeof(UnitOfWorkPostProcessor<,>));
                }
            });

            return services;
        }
    }
}
