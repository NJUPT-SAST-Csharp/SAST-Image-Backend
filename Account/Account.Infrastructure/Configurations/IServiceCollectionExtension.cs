using System.Reflection;
using Account.Application;
using Account.Application.Services;
using Account.Application.UserServices;
using Account.Domain.UserEntity.Services;
using Account.Infrastructure.ApplicationServices;
using Account.Infrastructure.DomainServices;
using Account.Infrastructure.EventBus;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Persistence.TypeConverters;
using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Dapper;
using Exceptions.Configurations;
using Exceptions.ExceptionHandlers;
using Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Primitives;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using SastImg.WebAPI.Configurations;
using Serilog;
using Shared.Storage.Configurations;
using StackExchange.Redis;

namespace Account.Infrastructure.Configurations
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .ConfigureAuth(configuration)
                .AddPersistence(configuration.GetConnectionString("AccountDb")!)
                .AddRepositories()
                .AddDistributedCache(configuration.GetConnectionString("DistributedCache")!)
                .AddEventBus(configuration)
                .AddExceptionHandlers()
                .AddStorages(configuration);

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

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    Description = "Authorization Header \r\nExample:'Bearer 123456789'",
                    Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Authorization" },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement { [scheme] = new List<string>() };
                options.AddSecurityRequirement(requirement);
                var xmlFilename = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            return services;
        }

        public static IServiceCollection ConfigureAuth(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddAuthorizationBuilder().AddBasicPolicies();
            services.ConfigureJwtAuthentication(options =>
            {
                options.SecKey =
                    configuration["Authentication:SecKey"]
                    ?? throw new NullReferenceException("Couldn't find 'SecKey'.");
                options.Algorithms =
                [
                    configuration["Authentication:Algorithm"]
                        ?? throw new NullReferenceException("Couldn't find 'Algorithm'.")
                ];
            });
            return services;
        }

        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseLoggerFactory(
                    new LoggerFactory().AddSerilog(
                        new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .MinimumLevel.Warning()
                            .CreateLogger()
                    )
                );
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            SqlMapper.AddTypeHandler(new UriStringConverter());
            SqlMapper.AddTypeHandler(new DateOnlyConverter());

            services.AddSingleton<IDbConnectionFactory>(
                _ => new DbConnectionFactory(connectionString)
            );

            return services;
        }

        private static IServiceCollection AddDistributedCache(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString)
            );
            services.AddScoped<IAuthCodeCache, RedisAuthCache>();
            return services;
        }

        private static IServiceCollection AddEventBus(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var config = configuration.GetSection("EventBus");

            services.AddCap(x =>
            {
                x.UseEntityFramework<AccountDbContext>();
                x.UseRabbitMQ(options =>
                {
                    options.HostName = config["HostName"]!;
                    options.UserName = config["UserName"]!;
                    options.Password = config["Password"]!;
                    options.Port = config.GetValue<int>("Port");
                });
            });

            services.AddScoped<IMessagePublisher, ExternalEventBus>();
            services.AddMediatR(
                mediat => mediat.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly)
            );
            services.AddScoped<IDomainEventPublisher, InternalEventBus>();
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandRequestSender, InternalEventBus>();

            return services;
        }

        private static IServiceCollection AddStorages(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.ConfigureImageStorage(configuration);
            return services;
        }

        private static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<DbNotFoundExceptionHandler>();
            services.AddExceptionHandler<DomainBusinessRuleInvalidExceptionHandler>();
            services.AddDefaultExceptionHandler();
            return services;
        }
    }
}
