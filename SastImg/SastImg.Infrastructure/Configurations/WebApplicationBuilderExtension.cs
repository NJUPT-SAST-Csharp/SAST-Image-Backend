using System.Text.Json.Serialization;
using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SastImg.Infrastructure.Configurations
{
    public static class WebApplicationBuilderExtension
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.ConfigureSwagger();
            }

            builder.Services.ConfigureOptions(configuration);

            builder.Services.AddLogging();

            builder.Services.ConfigureDatabase(configuration.GetConnectionString("SastimgDb")!);

            builder.Services.ConfigureCache(configuration.GetConnectionString("DistributedCache")!);

            builder.Services.ConfigureMediator();

            builder.Services.ConfigureEventBus(configuration);

            builder.Services.ConfigureStorage(configuration);

            builder.Services.ConfigureExceptionHandlers();

            builder
                .Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
                });

            builder.Services.ConfigureJwtAuthentication(options =>
            {
                options.SecKey = configuration["Authentication:SecKey"]!;
                options.Algorithms = [configuration["Authentication:Algorithm"]!];
            });
            builder.Services.AddAuthorizationBuilder().AddBasicPolicies();
        }
    }
}
