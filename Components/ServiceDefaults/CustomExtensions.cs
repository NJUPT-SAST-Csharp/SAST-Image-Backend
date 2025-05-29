using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class CustomExtensions
{
    public static IHostApplicationBuilder EnrichPersistence<TContext>(
        this IHostApplicationBuilder builder,
        Action<NpgsqlEntityFrameworkCorePostgreSQLSettings>? configureSettings = null
    )
        where TContext : DbContext
    {
        builder.EnrichNpgsqlDbContext<TContext>(configureSettings);

        return builder;
    }

    public static IHostApplicationBuilder EnrichPersistence<TWriteContext, TReadContext>(
        this IHostApplicationBuilder builder,
        Action<NpgsqlEntityFrameworkCorePostgreSQLSettings>? configureSettings = null
    )
        where TWriteContext : DbContext
        where TReadContext : DbContext
    {
        builder.EnrichNpgsqlDbContext<TWriteContext>(configureSettings);
        builder.EnrichNpgsqlDbContext<TReadContext>(configureSettings);

        return builder;
    }
}
