using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SastImg.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static void ConfigureServices(
            this WebApplicationBuilder builder,
            IConfigurationRoot configuration
        )
        {
            builder.Services.TryAddSingleton(configuration);
            builder.Services.ConfigureOptions(builder.Configuration);
            builder.Services.AddLogging();
            builder.Services.ConfigureDatabase(
                configuration.GetConnectionString("SastimgDb")
                    ?? throw new Exception("The connection string \"SastimgDb\" is null.")
            );
            builder.Services.ConfigureCache(
                configuration.GetConnectionString("DistributedCache")
                    ?? throw new Exception("The connection string \"DistributedCache\" is null.")
            );
            builder.Services.ConfigureMediator();
            builder.Services.ConfigureExceptionHandlers();

            builder.Services.ConfigureSwagger();

            builder.Services.AddControllers();

            builder.Services.ConfigureJwtAuthentication(options =>
            {
                options.SecKey =
                    configuration["Authentication:SecKey"]
                    ?? throw new NullReferenceException("Couldn't find 'SecKey'.");
                options.Algorithms =
                [
                    configuration["Authentication:Algorithm"]
                        ?? throw new NullReferenceException("Couldn't find 'Algorithm'.")
                ];
            });
            builder.Services.AddAuthorizationBuilder().AddBasicPolicies();
        }
    }
}
