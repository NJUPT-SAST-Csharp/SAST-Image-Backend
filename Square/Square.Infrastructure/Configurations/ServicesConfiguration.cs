using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigurePersistence(builder.Configuration);

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

            return services;
        }
    }
}
