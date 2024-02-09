using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Primitives;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate;
using SNS.Domain.UserEntity;
using SNS.Infrastructure.DomainRepositories;
using SNS.Infrastructure.EventBus;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureDbContext(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<SNSDbContext>(
                options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
            );
            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandRequestSender, InternalEventBus>();
            services.AddScoped<IDomainEventPublisher, InternalEventBus>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
            });
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
    }
}
