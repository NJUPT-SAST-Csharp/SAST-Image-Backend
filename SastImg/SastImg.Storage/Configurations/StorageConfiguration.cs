using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SastImg.Storage.Implements;
using SastImg.Storage.Options;
using SastImg.Storage.Services;

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
            services.AddScoped<IImageClient, ImageClient>();

            services.Configure<OssOptions>(configuration.GetRequiredSection(OssOptions.Position));
            return services;
        }
    }
}
