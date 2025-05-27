using Account.Application.FileServices;
using Account.Application.Services;
using Account.Application.UserServices;
using Account.Domain.UserEntity.Services;
using Account.Infrastructure.ApplicationServices;
using Account.Infrastructure.DomainServices;
using Account.Infrastructure.EventBus;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Persistence.Storages;
using Account.Infrastructure.Persistence.TypeConverters;
using Dapper;
using Exceptions.Configurations;
using Exceptions.ExceptionHandlers;
using Messenger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Primitives;
using Shared.Storage.Configurations;

namespace Account.Infrastructure.Configurations;

public static class IServiceCollectionExtension
{
    public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

        services
            .AddRepositories()
            .AddDistributedCache()
            .AddEventBus(configuration)
            .AddExceptionHandlers()
            .AddStorages(configuration)
            .AddPersistence(configuration.GetConnectionString("AccountDb")!)
            .AddPrimitives(options =>
                options.AddUnitOfWorkWithDbContext<AccountDbContext>().AddDefaultExceptionHandler()
            );

        services.AddScoped<IUserUniquenessChecker, UserUniquenessChecker>();
        services.AddScoped<IAuthCodeSender, EmailCodeSender>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        string connectionString
    )
    {
        SqlMapper.AddTypeHandler(new UriStringConverter());
        SqlMapper.AddTypeHandler(new DateOnlyConverter());

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));

        return services;
    }

    private static IServiceCollection AddDistributedCache(this IServiceCollection services)
    {
        services.AddScoped<IAuthCodeCache, RedisAuthCache>();
        return services;
    }

    private static IServiceCollection AddEventBus(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddCap(x =>
        {
            string connectionString =
                configuration.GetConnectionString("RabbitMQ") ?? throw new NullReferenceException();

            x.UseEntityFramework<AccountDbContext>();
            x.UseRabbitMQ(options =>
            {
                options.ConnectionFactoryOptions = options => options.Uri = new(connectionString);
            });
        });

        services.AddScoped<IMessagePublisher, ExternalEventBus>();
        services.AddMediator(o => o.ServiceLifetime = ServiceLifetime.Scoped);

        return services;
    }

    private static IServiceCollection AddStorages(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IHeaderStorageRepository, HeaderStorageRepository>();
        services.AddScoped<IAvatarStorageRepository, AvatarStorageRepository>();

        services.AddStorageClient(options => options.FolderPath = configuration["StoragePath"]!);
        return services;
    }

    private static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<DbNotFoundExceptionHandler>();
        services.AddDefaultExceptionHandler();
        return services;
    }
}
