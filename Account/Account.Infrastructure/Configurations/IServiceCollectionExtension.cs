using System.Text;
using Account.Application.Services;
using Account.Application.Users.Login;
using Account.Application.Users.Repository;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Services;
using Account.WebAPI.Endpoints.Login;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

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
                .AddAuthenticationHandle(configuration)
                .AddEndpointHandlers()
                .AddValidators()
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
        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseLoggerFactory(
                    new LoggerFactory().AddSerilog(
                        new LoggerConfiguration()
                            .Enrich
                            .FromLogContext()
                            .WriteTo
                            .Console()
                            .MinimumLevel
                            .Warning()
                            .CreateLogger()
                    )
                );
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        /// <summary>
        /// Add password hash provider
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddPasswordHasher(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            return services;
        }

        private static IServiceCollection AddEndpointHandlers(this IServiceCollection services)
        {
            services.AddScoped<LoginEndpointHandler>();
            return services;
        }

        private static IServiceCollection AddAuthenticationHandle(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.RequireAuthenticatedSignIn = true;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.Default.GetBytes("233")
                        ),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(1),
                        ValidAlgorithms =  ["SHA256", "Argon2"]
                    };
                });

            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            return services;
        }
    }
}
