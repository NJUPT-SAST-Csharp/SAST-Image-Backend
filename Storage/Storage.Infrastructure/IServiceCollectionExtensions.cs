using Microsoft.Extensions.DependencyInjection;
using Minio;
using Storage.Application.Service;
using Storage.Infrastructure.Service;

namespace Storage.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        Action<StorageOptions>? optionsBuilder = null
    )
    {
        return services.AddMinIO(optionsBuilder).AddCache().AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileStorage, FileStorage>();
        services.AddSingleton<ITokenRepository, TokenRepository>();

        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services)
    {
        // TODO: Implement caching mechanism if needed
        return services;
    }

    private static IServiceCollection AddMinIO(
        this IServiceCollection services,
        Action<StorageOptions>? optionsBuilder = null
    )
    {
        StorageOptions options = new();
        optionsBuilder?.Invoke(options);

        services.AddSingleton(options);
        services.AddMinio(client =>
            client
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .Build()
        );

        return services;
    }
}
