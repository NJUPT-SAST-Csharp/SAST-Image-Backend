using System.Data.Common;
using Dapper;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Primitives;
using SNS.Application.GetFollowCount;
using SNS.Application.GetFollowers;
using SNS.Application.GetFollowing;
using SNS.Domain.Bookmarks;
using SNS.Domain.Follows;
using SNS.Infrastructure.Managers;
using SNS.Infrastructure.Persistence;
using SNS.Infrastructure.Persistence.TypeConverters;
using SNS.Infrastructure.Query;

namespace SNS.Infrastructure.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

        services
            .ConfigureEventBus(builder.Configuration)
            .ConfigurePersistence(builder.Configuration)
            .AddPrimitives(options =>
            {
                options.AddUnitOfWorkWithDbContext<SNSDbContext>();
            });

        return services;
    }

    public static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        string connectionString
    )
    {
        return services;
    }

    public static IServiceCollection ConfigurePersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connectionString = configuration.GetConnectionString("SNSDb")!;

        //services.AddDbContext<SNSDbContext>(
        //    options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
        //);

        services.AddSingleton<DbDataSource>(_ =>
            new NpgsqlDataSourceBuilder(connectionString).Build()
        );
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(_ => new DbConnectionFactory(
            connectionString
        ));
        SqlMapper.AddTypeHandler(new UriStringConverter());

        services.AddScoped<IFollowerRepository, FollowRepository>();
        services.AddScoped<IFollowingRepository, FollowRepository>();
        services.AddScoped<IFollowCountRepository, FollowRepository>();

        services.AddScoped<IBookmarkManager, BookmarkManager>();
        services.AddScoped<IFollowManager, FollowManager>();

        services.AddSingleton<IDomainEventContainer, DomainEventContainer>();

        return services;
    }

    public static IServiceCollection ConfigureEventBus(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var config = configuration.GetSection("EventBus");

        services.AddCap(x =>
        {
            string connectionString =
                configuration.GetConnectionString("RabbitMQ") ?? throw new NullReferenceException();

            Uri url = new(connectionString);

            x.UseEntityFramework<SNSDbContext>();
            x.UseRabbitMQ(options =>
            {
                options.ConnectionFactoryOptions = options => options.Uri = url;
            });
        });

        return services;
    }
}
