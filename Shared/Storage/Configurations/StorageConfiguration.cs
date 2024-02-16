using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SastImg.Application.ImageServices.AddImage;
using Shared.Storage.Implements;
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
            services.TryAddSingleton<IOssClientFactory, OssClientFactory>();
            services.AddScoped<IImageStorageClient, ImageClient>();
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
            services.TryAddSingleton<IOssClientFactory, OssClientFactory>();
            //services.AddScoped<IHeaderStorageClient, HeaderClient>();
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
            services.TryAddSingleton<IOssClientFactory, OssClientFactory>();
            //services.AddScoped<IAvatarStorageClient, AvatarClient>();
            services.Configure<AvatarOssOptions>(
                configuration.GetRequiredSection(AvatarOssOptions.Position)
            );
            return services;
        }
    }
}
