using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Storage.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(
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
