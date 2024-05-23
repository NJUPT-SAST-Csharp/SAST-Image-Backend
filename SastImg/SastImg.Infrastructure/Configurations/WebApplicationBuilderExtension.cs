using System.Text.Json;
using System.Text.Json.Serialization;
using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Primitives;
using SastImg.Infrastructure.Persistence;

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

            builder.Services.ConfigureMessageQueue(configuration);

            builder.Services.ConfigureStorage(configuration);

            builder.Services.ConfigureExceptionHandlers();

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonNumberConverter());
                options.SerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
            });

            builder
                .Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonNumberConverter());
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
                });

            builder.Services.AddPrimitives(options =>
                options
                    .AddUnitOfWorkWithDbContext<SastImgDbContext>()
                    .AddResolverFromAssembly(Application.AssemblyReference.Assembly)
            );

            builder.Services.ConfigureJwtAuthentication(options =>
            {
                options.SecKey = configuration["Authentication:SecKey"]!;
                options.Algorithms = [configuration["Authentication:Algorithm"]!];
            });
            builder.Services.AddAuthorizationBuilder().AddBasicPolicies();
        }
    }

    file class JsonNumberConverter : JsonConverter<long>
    {
        public override long Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            long num = reader.GetInt64();
            return num;
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            if (value > 9007199254740993)
                writer.WriteStringValue(value.ToString());
            else
                writer.WriteNumberValue(value);
        }
    }
}
