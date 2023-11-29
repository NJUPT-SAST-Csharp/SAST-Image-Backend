using Account.Application.Services;
using Account.Application.Users.Login;
using Account.Application.Users.Repository;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddEndpointHandlers()
                .AddPasswordHasher()
                .AddPersistence(
                    configuration.GetConnectionString("AccountDb")
                        ?? throw new NullReferenceException()
                );

            return services;
        }

        /// <summary>
        /// Configure database & persistence
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        internal static IServiceCollection AddPersistence(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<AccountDbContext>(
                options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
            );
            services.AddTransient<IUserRepository, UserRepository>();
            return services;
        }

        /// <summary>
        /// Add password hash provider
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddPasswordHasher(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            return services;
        }

        internal static IServiceCollection AddEndpointHandlers(this IServiceCollection services)
        {
            services.AddScoped<LoginEndpointHandler>();
            return services;
        }
    }
}
