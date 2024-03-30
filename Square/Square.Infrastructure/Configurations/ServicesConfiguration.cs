using System.Reflection;
using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Primitives;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using Serilog;
using Shared.Storage.Configurations;
using Square.Application.Behaviors;
using Square.Application.TopicServices;
using Square.Domain.ColumnAggregate;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.ApplicationServices;
using Square.Infrastructure.DomainServices;
using Square.Infrastructure.EventBus;
using Square.Infrastructure.Persistence;
using Square.Infrastructure.Persistence.Storages;
using Storage.Options;

namespace Square.Infrastructure.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
        {
            builder
                .Services.ConfigureMediatR()
                .ConfigureDomainServices()
                .ConfigureApplicationServices()
                .ConfigurePersistence(builder.Configuration)
                .ConfigureAuth(builder.Configuration);

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.ConfigureSwagger();
            }

            return builder.Services;
        }

        private static IServiceCollection ConfigurePersistence(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<SquareDbContext>(
                options =>
                    options
                        .UseNpgsql(configuration.GetConnectionString("SquareDb"))
                        .UseSnakeCaseNamingConvention()
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()))
            );

            services.AddDbContext<SquareQueryDbContext>(
                options =>
                    options
                        .UseNpgsql(configuration.GetConnectionString("SquareDb"))
                        .UseSnakeCaseNamingConvention()
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()))
            );

            services.AddStorageClient(
                configuration.GetRequiredSection("Storage").Get<StorageOptions>()
                    ?? throw new NullReferenceException("Couldn't find 'Storage' configuration")
            );

            return services;
        }

        private static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();

            services.AddScoped<ITopicUniquenessChecker, TopicUniquenessChecker>();

            return services;
        }

        private static IServiceCollection ConfigureApplicationServices(
            this IServiceCollection services
        )
        {
            services.AddSingleton<ITopicImageStorage, TopicImageStorage>();
            services.AddScoped<ITopicQueryRepository, TopicQueryRepository>();

            return services;
        }

        private static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandRequestSender, InternalEventBus>();
            services.AddScoped<IDomainEventPublisher, InternalEventBus>();

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining<Topic>();
                options.RegisterServicesFromAssemblyContaining<TopicModel>();
                options.AddOpenBehavior(typeof(CommandLoggingBehavior<,>));
                options.AddOpenBehavior(typeof(QueryLoggingBehavior<,>));
                options.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });

            return services;
        }

        private static IServiceCollection ConfigureAuth(
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

        private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
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
    }
}
