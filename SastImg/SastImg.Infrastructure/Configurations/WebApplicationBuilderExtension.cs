using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Primitives;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.Configurations;

public static class WebApplicationBuilderExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

        services.AddPrimitives(options =>
            options.AddUnitOfWorkWithDbContext<SastImgDbContext>().AddDefaultExceptionHandler()
        );

        services.ConfigureOptions(configuration);

        services.AddLogging();

        services.ConfigureDatabase(configuration.GetConnectionString("SastimgDb")!);

        services.ConfigureMessageQueue(configuration);

        services.ConfigureStorage(configuration);

        services.ConfigureExceptionHandlers();

        services.ConfigureHttpJsonOptions(options =>
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
