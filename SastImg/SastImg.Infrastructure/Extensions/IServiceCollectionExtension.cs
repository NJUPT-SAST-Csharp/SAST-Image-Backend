using System.Reflection;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SastImg.Application.Services;
using SastImg.Application.Services.EventBus;
using SastImg.Infrastructure.Cache;
using SastImg.Infrastructure.Event;
using SastImg.Infrastructure.Persistence;
using SastImg.Infrastructure.Persistence.TypeConverters;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureOptions(this IServiceCollection services)
        {
            // TODO: Add options
            return services;
        }

        public static IServiceCollection ConfigureDatabase(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<SastImgDbContext>(options =>
            {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });
            SqlMapper.AddTypeHandler(new UriStringConverter());
            services.AddSingleton<IDbConnectionProvider>(
                new DbConnectionProvider(connectionString)
            );
            return services;
        }

        public static IServiceCollection ConfigureRedis(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString)
            );
            return services;
        }

        public static IServiceCollection ConfigureCache(this IServiceCollection services)
        {
            services.AddSingleton<ICache, RedisCache>();
            return services;
        }

        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddSingleton<IExternalEventBus, ExternalEventBus>();
            services.AddSingleton<IInternalEventBus, InternalEventBus>();
            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly)
            );
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
    }
}
