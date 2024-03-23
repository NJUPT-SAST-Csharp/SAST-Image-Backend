using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Shared.Storage.Options;
using Storage.Clients;
using Storage.Options;

namespace Shared.Storage.Configurations
{
    public static class StorageConfiguration
    {
        public static IServiceCollection ConfigureImageStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.TryAddSingleton<IStorageClientFactory, StorageClientFactory>();
            services.AddScoped<ImageClient>();
            services.Configure<ImageOssOptions>(
                configuration.GetRequiredSection(ImageOssOptions.Position)
            );
            return services;
        }

        public static IServiceCollection ConfigureHeaderStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.TryAddSingleton<IStorageClientFactory, StorageClientFactory>();
            services.AddScoped<HeaderClient>();
            services.Configure<HeaderOssOptions>(
                configuration.GetRequiredSection(HeaderOssOptions.Position)
            );
            return services;
        }

        public static IServiceCollection ConfigureAvatarStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.TryAddSingleton<IStorageClientFactory, StorageClientFactory>();
            services.AddScoped<AvatarClient>();
            services.Configure<AvatarOssOptions>(
                configuration.GetRequiredSection(AvatarOssOptions.Position)
            );
            return services;
        }

        public static IServiceCollection ConfigureTopicImageStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.TryAddSingleton<IStorageClientFactory, StorageClientFactory>();
            return services;
        }

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
