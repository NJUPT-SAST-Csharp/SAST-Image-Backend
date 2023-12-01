using System.Text;
using Account.Application.Account.Login;
using Account.Application.Account.Register;
using Account.Application.Account.Repository;
using Account.Application.Services;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Services;
using Account.WebAPI.Endpoints.Login;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Primitives.Common;
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
                .ConfigureAuthentication(configuration)
                .ConfigureAuthorization(configuration)
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
        /// Configure password hash provider
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddPasswordHasher(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            return services;
        }

        /// <summary>
        /// Configure endpoint handlers that provided when requests are received from endpoints.
        /// </summary>
        private static IServiceCollection AddEndpointHandlers(this IServiceCollection services)
        {
            services.AddScoped<LoginEndpointHandler>();
            services.AddScoped<SendCodeEndpointHandler>();
            return services;
        }

        /// <summary>
        /// Configure authentication module (jwt)
        /// </summary>
        private static IServiceCollection ConfigureAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication()
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

        private static IServiceCollection ConfigureAuthorization(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthorizationBuilder()
                .AddPolicy(
                    AuthorizationRoles.User,
                    policy => policy.RequireAuthenticatedUser().RequireClaim("role", "user")
                );
            return services;
        }

        /// <summary>
        /// Configure model validators. (mainly for request)
        /// </summary>
        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<SendCodeRequest>, SendCodeRequestValidator>();
            return services;
        }
    }
}
