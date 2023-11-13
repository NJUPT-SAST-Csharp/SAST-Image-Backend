using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SastImg.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static void ConfigureServices(
            this WebApplicationBuilder builder,
            IConfigurationRoot configuration
        )
        {
            builder
                .Services
                .AddSingleton(configuration)
                .AddLogging()
                .ConfigureDatabase(
                    configuration.GetConnectionString("SastimgDb")
                        ?? throw new Exception("The connection string \"SastimgDb\" is null.")
                )
                .ConfigureRedis(
                    configuration.GetConnectionString("DistributedCache")
                        ?? throw new Exception(
                            "The connection string \"DistributedCache\" is null."
                        )
                )
                .ConfigureCache()
                .ConfigureMediator();

            builder.Services.ConfigureSwagger();
            builder.Services.AddControllers();
        }
    }
}
