using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Application.Services;
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
            .AddDefaultExceptionHandler()
            .AddPersistence<AccountDbContext>(configuration.GetConnectionString("AccountDb")!);

        SqlMapper.AddTypeHandler(new UriStringConverter());
        SqlMapper.AddTypeHandler(new DateOnlyConverter());
        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(
            configuration.GetConnectionString("AccountDb")!
        ));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();

        services.AddScoped<IUsernameUniquenessChecker, UserUniquenessChecker>();
        services.AddScoped<IJwtTokenGenerator, JwtProvider>();

        services.AddSingleton<PasswordProcessor>();
        services.AddSingleton<IPasswordGenerator>(sp => sp.GetRequiredService<PasswordProcessor>());
        services.AddSingleton<IPasswordValidator>(sp => sp.GetRequiredService<PasswordProcessor>());

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        return services;
    }
}
