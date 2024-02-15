using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SastImg.Application.ImageServices.AddImage;
using Shared.Storage.Implements;
using Shared.Storage.Options;
using Storage.Clients;

namespace Shared.Storage.Configurations
{
    public static class StorageConfiguration
    {
        public static IServiceCollection ConfigureImageStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddSingleton<IOssClientFactory, OssClientFactory>();
            services.AddScoped<IImageStorageClient, ImageClient>();
            services.Configure<ImageOssOptions>(
                configuration.GetRequiredSection(ImageOssOptions.Position)
            );
            return services;
        }

        public static IServiceCollection ConfigureHeaderStorage(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection ConfigureAvatarStorage(this IServiceCollection services)
        {
            return services;
        }
    }
}
