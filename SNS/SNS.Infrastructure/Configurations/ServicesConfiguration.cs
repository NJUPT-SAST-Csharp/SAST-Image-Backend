using System.Data.Common;
using System.Reflection;
using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Primitives;
using Shared.Primitives.DomainEvent;
using SNS.Application.GetFollowCount;
using SNS.Application.GetFollowers;
using SNS.Application.GetFollowing;
using SNS.Domain.Bookmarks;
using SNS.Domain.Follows;
using SNS.Infrastructure.Managers;
using SNS.Infrastructure.Persistence;
using SNS.Infrastructure.Persistence.TypeConverters;
using SNS.Infrastructure.Query;

namespace SNS.Infrastructure.Configurations
{
    public static class ServicesConfiguration
    {
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

            services.AddDbContext<SNSDbContext>(
                options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
            );

            services.AddSingleton<DbDataSource>(
                _ => new NpgsqlDataSourceBuilder(connectionString).Build()
            );
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(
                _ => new DbConnectionFactory(connectionString)
            );
            SqlMapper.AddTypeHandler(new UriStringConverter());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
                x.UseEntityFramework<SNSDbContext>();
                x.UseRabbitMQ(options =>
                {
                    options.Port = config.GetValue<int>("Port");
                    options.HostName = config["HostName"]!;
                    options.UserName = config["UserName"]!;
                    options.Password = config["Password"]!;
                });
            });

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
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
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
            services.ConfigureJwtAuthentication(options =>
            {
                options.SecKey = configuration["Authentication:SecKey"]!;
                options.Algorithms = [configuration["Authentication:Algorithm"]!];
            });
            services.AddAuthorizationBuilder().AddBasicPolicies();
            return services;
        }
    }
}
