using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Storage.Implements;
using SastImg.Storage.Options;

namespace SastImg.Storage.Configurations
{
    public static class StorageConfiguration
    {
        public static IServiceCollection ConfigureStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddSingleton<IOssClientFactory, OssClientFactory>();
            services.AddScoped<IImageStorageClient, ImageClient>();
            services.Configure<OssOptions>(configuration.GetRequiredSection(OssOptions.Position));
            return services;
        }
    }
}
