using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Minio;
using Storage.Application.Service;
using Storage.Infrastructure.Service;

namespace Storage.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static WebApplicationBuilder AddServices(
        this WebApplicationBuilder builder,
        Action<StorageConfiguration>? optionsBuilder = null
    )
    {
        var services = builder.Services;

        services.AddMediator();
        services.AddSingleton<IFileStorage, FileStorage>();
        services.AddSingleton<ITokenRepository, TokenRepository>();

        services.AddSingleton<TokenProcessor>();
        services.AddSingleton<ITokenIssuer>(sp => sp.GetRequiredService<TokenProcessor>());
        services.AddSingleton<ITokenValidator>(sp => sp.GetRequiredService<TokenProcessor>());

        services.AddOptions<StorageConfiguration>().BindConfiguration(nameof(StorageConfiguration));

        return builder;
    }

    public static IServiceCollection AddMinIO(
        this IServiceCollection services,
        Action<MinIOConfiguration>? optionsBuilder = null
    )
    {
        var config = services
            .AddOptions<MinIOConfiguration>()
            .BindConfiguration(nameof(MinIOConfiguration));

        if (optionsBuilder is not null)
            config.PostConfigure(optionsBuilder);

        services.TryAddSingleton<IMinioClient>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<MinIOConfiguration>>().Value;
            var minio = new MinioClient()
                .WithEndpoint(config.Endpoint)
                .WithCredentials(config.AccessKey, config.SecretKey)
                .WithSSL(false)
                .Build();

            return minio;
        });

        return services;
    }

    public static WebApplicationBuilder AddMinIO(
        this WebApplicationBuilder builder,
        Action<MinIOConfiguration>? optionsBuilder = null
    )
    {
        builder.Services.AddMinIO(optionsBuilder);
        return builder;
    }
}
