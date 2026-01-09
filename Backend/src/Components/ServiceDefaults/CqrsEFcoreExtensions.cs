using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Hosting;

public static class CqrsEFcoreExtensions
{
    public static IHostApplicationBuilder AddPersistenceSupport<TContext>(
        this IHostApplicationBuilder builder,
        Action<NpgsqlEntityFrameworkCorePostgreSQLSettings>? configureSettings = null
    )
        where TContext : DbContext
    {
        builder.EnrichNpgsqlDbContext<TContext>(configureSettings);

        return builder;
    }

    public static IHostApplicationBuilder AddPersistenceSupport<TWriteContext, TReadContext>(
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
