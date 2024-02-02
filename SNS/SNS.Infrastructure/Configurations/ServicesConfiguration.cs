using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using SNS.Domain.UserEntity;
using SNS.Infrastructure.EventBus;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services)
        {
            services.AddDbContext<SNSDbContext>(
                options =>
                    options
                        .UseNpgsql(
                            "Host=localhost;Port=5432;Database=sastimg_sns;Username=postgres;Password=150524"
                        )
                        .UseSnakeCaseNamingConvention()
            );
            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
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
    }
}
