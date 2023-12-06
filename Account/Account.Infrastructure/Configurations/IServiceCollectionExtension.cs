using System.Text;
using Account.Application.Account.Login;
using Account.Application.Account.Register;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.User.Repositories;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Primitives.Common;
using Serilog;
using StackExchange.Redis;

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
                .AddPersistence(
                    configuration.GetConnectionString("AccountDb")
                        ?? throw new NullReferenceException(
                            "Couldn't find the database connection string."
                        )
                )
                .AddDistributedCache(
                    configuration.GetConnectionString("DistributedCache")
                        ?? throw new NullReferenceException(
                            "Couldn't find the cache connection string."
                        )
                );

            services.AddScoped<ITokenSender, EmailTokenSender>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            return services;
        }

        /// <summary>
        /// Configure endpoint handlers that provided when requests are received from endpoints.
        /// </summary>
        private static IServiceCollection AddEndpointHandlers(this IServiceCollection services)
        {
            services.AddScoped<IEndpointHandler<LoginRequest>, LoginEndpointHandler>();
            services.AddScoped<IEndpointHandler<SendCodeRequest>, SendCodeEndpointHandler>();
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

        /// <summary>
        /// Configure authentication module based on jwt
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

        /// <summary>
        /// Configure authorization module based on policy
        /// </summary>
        private static IServiceCollection ConfigureAuthorization(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthorizationBuilder()
                .AddPolicy(
                    AuthorizationRoles.User,
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("role", "user")
                            .RequireClaim("username")
                            .RequireClaim("id")
                );
            return services;
        }

        /// <summary>
        /// Add database & persistence provider
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

            services.AddScoped<IUserQueryRepository, UserRepository>();
            services.AddScoped<IUserCommandRepository, UserRepository>();
            services.AddScoped<IUserCheckRepository, UserRepository>();
            return services;
        }

        /// <summary>
        /// Add distributed cache provider (redis)
        /// </summary>
        private static IServiceCollection AddDistributedCache(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString)
            );
            services.AddScoped<IAuthCache, RedisAuthCache>();
            return services;
        }
    }
}
