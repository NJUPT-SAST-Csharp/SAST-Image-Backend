using System.Data.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Primitives;

namespace Persistence;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence<TContext>(
        this IServiceCollection services,
        string connectionString,
        Action<DbContextOptionsBuilder>? optionsAction = null
    )
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
            optionsAction?.Invoke(options);
        });

        services.AddUnitOfWork<DefaultUnitOfWork<TContext>>();

        return services;
    }

    public static IServiceCollection AddPersistence<TWriteContext, TQueryContext>(
        this IServiceCollection services,
        string connectionString,
        Action<DbContextOptionsBuilder>? optionsAction = null
    )
        where TWriteContext : DbContext
        where TQueryContext : DbContext
    {
        services.TryAddScoped<DbConnection>(_ => new NpgsqlConnection(connectionString));
        services
            .AddDbContext<TWriteContext>(
                (services, options) =>
                {
                    options.UseNpgsql(services.GetRequiredService<DbConnection>());
                    optionsAction?.Invoke(options);
                }
            )
            .AddDbContext<TQueryContext>(
                (services, options) =>
                {
                    options.UseNpgsql(services.GetRequiredService<DbConnection>());
                    optionsAction?.Invoke(options);
                }
            );

        services.AddUnitOfWork<DefaultUnitOfWork<TWriteContext, TQueryContext>>();

        return services;
    }

    private static void AddUnitOfWork<TUnitOfWork>(this IServiceCollection services)
        where TUnitOfWork : class, IUnitOfWork
    {
        services.AddScoped<IUnitOfWork, TUnitOfWork>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPostProcessor<,>));
    }
}
