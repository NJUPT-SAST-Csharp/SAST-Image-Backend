using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class RedisExtensions
{
    public static THost AddRedisSupport<THost>(this THost host)
        where THost : IHostApplicationBuilder
    {
        host.AddKeyedRedisClient(nameof(StackExchange.Redis));
        host.AddRedisClient(nameof(StackExchange.Redis));
        return host;
    }
}
