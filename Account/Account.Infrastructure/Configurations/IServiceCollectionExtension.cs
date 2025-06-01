using Account.Application.Services;
using Account.Application.UserServices;
using Account.Domain.UserEntity.Services;
using Account.Infrastructure.ApplicationServices;
using Account.Infrastructure.DomainServices;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Persistence.TypeConverters;
using Dapper;
using Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

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
            .AddDefaultExceptionHandler()
            .AddPersistence<AccountDbContext>(configuration.GetConnectionString("AccountDb")!)
            .AddPersistence(configuration.GetConnectionString("AccountDb")!);

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
}
