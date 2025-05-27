using Microsoft.Extensions.DependencyInjection;

namespace Primitives;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddPrimitives(
        this IServiceCollection services,
        Action<PrimitivesOptions>? optionsBuilder = null
    )
    {
        PrimitivesOptions options = new(services);
        optionsBuilder?.Invoke(options);
        return services;
    }
}
