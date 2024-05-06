using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using Exceptions.Configurations;
using Exceptions.ExceptionHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Primitives;
using Primitives.Rule;
using Serilog;
using Shared.Storage.Configurations;
using Square.Application.CategoryServices;
using Square.Application.ColumnServices;
using Square.Application.TopicServices;
using Square.Domain.CategoryAggregate;
using Square.Domain.ColumnAggregate;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.ApplicationServices;
using Square.Infrastructure.DomainServices;
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
                .Services.AddPrimitives(options =>
                {
                    options
                        .AddResolversFromAssemblyContaining<Topic>()
                        .AddResolversFromAssemblyContaining<TopicModel>()
                        .AddUnitOfWorkWithDbContext<SquareDbContext, SquareQueryDbContext>();
                    options.AutoCommitAfterCommandHandled = true;
                })
                .ConfigureDomainServices()
                .ConfigureApplicationServices()
                .ConfigureExceptionHandlers()
                .ConfigurePersistence(builder.Configuration)
                .ConfigureAuth(builder.Configuration);

            return builder.Services;
        }

        private static IServiceCollection ConfigureExceptionHandlers(
            this IServiceCollection services
        )
        {
            services.AddProblemDetails();

            services.AddExceptionHandler<DomainBusinessRuleInvalidExceptionHandler>();
            services.AddExceptionHandler<DomainObjectValidationExceptionHandler>();
            services.AddExceptionHandler<DbNotFoundExceptionHandler>();
            services.AddDefaultExceptionHandler();

            return services;
        }

        private static IServiceCollection ConfigurePersistence(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<SquareDbContext>(
                options =>
                    options
                        .EnableSensitiveDataLogging()
                        .UseNpgsql(configuration.GetConnectionString("SquareDb"))
                        .UseSnakeCaseNamingConvention()
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()))
            );

            services.AddDbContext<SquareQueryDbContext>(
                options =>
                    options
                        .EnableSensitiveDataLogging()
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
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<ITopicUniquenessChecker, TopicUniquenessChecker>();
            services.AddScoped<ICategoryUniquenessChecker, CategoryUniquenessChecker>();

            return services;
        }

        private static IServiceCollection ConfigureApplicationServices(
            this IServiceCollection services
        )
        {
            services.AddSingleton<IColumnImageStorage, TopicImageStorage>();
            services.AddScoped<ITopicQueryRepository, TopicQueryRepository>();
            services.AddScoped<IColumnQueryRepository, ColumnQueryRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();

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
    }
}
