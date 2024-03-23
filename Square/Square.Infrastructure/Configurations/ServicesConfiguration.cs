using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Primitives;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using Shared.Storage.Configurations;
using Square.Application.SeedWorks;
using Square.Application.TopicServices;
using Square.Domain.TopicAggregate;
using Square.Infrastructure.DomainRepositories;
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
                .ConfigureRepositories()
                .ConfigurePersistence(builder.Configuration);

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
            );

            services.AddStorageClient(
                configuration.GetRequiredSection("Storage").Get<StorageOptions>()
                    ?? throw new NullReferenceException("Couldn't find 'Storage' configuration")
            );

            return services;
        }

        private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITopicImageStorageRepository, TopicImageStorageRepository>();

            return services;
        }

        private static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandRequestSender, InternalEventBus>();
            services.AddScoped<IDomainEventPublisher, InternalEventBus>();

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining<RequesterInfo>();
            });

            return services;
        }
    }
}
