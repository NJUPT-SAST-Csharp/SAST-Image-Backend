using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Storage.Clients;
using Storage.Options;

namespace Shared.Storage.Configurations
{
    public static class StorageConfiguration
    {
        public static IServiceCollection AddStorageClient(
            this IServiceCollection services,
            StorageOptions options
        )
        {
            services.TryAddSingleton(
                new DefaultObjectPoolProvider().CreateStringBuilderPool(128, 512)
            );

            services.TryAddSingleton(options);
            services.TryAddSingleton<IProcessClient, ProcessClient>();
            services.TryAddSingleton<IStorageClient, StorageClient>();
            return services;
        }

        public static IServiceCollection AddStorageClient(
            this IServiceCollection services,
            Action<StorageOptions> storageOptionsBuilder
        )
        {
            StorageOptions options = new();
            storageOptionsBuilder(options);

            services.TryAddSingleton(
                new DefaultObjectPoolProvider().CreateStringBuilderPool(128, 512)
            );

            services.TryAddSingleton(options);
            services.TryAddSingleton<IProcessClient, ProcessClient>();
            services.TryAddSingleton<IStorageClient, StorageClient>();

            return services;
        }
    }
}
