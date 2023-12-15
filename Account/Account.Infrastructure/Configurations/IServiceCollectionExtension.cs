using System.Reflection;
using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Account.Application.Endpoints.UserEndpoints.ChangeProfile;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.RoleEntity.Repositories;
using Account.Entity.UserEntity.Repositories;
using Account.Infrastructure.Persistence;
using Account.Infrastructure.Services;
using Auth.Authentication.Extensions;
using Auth.Authorization.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
                .ConfigureAuth(configuration)
                .ConfigureEndpoints()
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
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
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

        public static IServiceCollection ConfigureEndpoints(this IServiceCollection services)
        {
            services
                .RegisterEndpointResolver<
                    LoginRequest,
                    LoginEndpointHandler,
                    LoginRequestValidator
                >()
                .RegisterEndpointResolver<
                    SendRegistrationCodeRequest,
                    SendRegistrationCodeEndpointHandler,
                    SendRegistrationCodeRequestValidator
                >()
                .RegisterEndpointResolver<
                    VerifyRegistrationCodeRequest,
                    VerifyRegistrationCodeEndpointHandler,
                    VerifyRegistrationCodeRequestValidator
                >()
                .RegisterEndpointResolver<
                    SendForgetCodeRequest,
                    SendForgetCodeEndpointHandler,
                    SendForgetCodeRequestValidator
                >()
                .RegisterEndpointResolver<
                    VerifyForgetCodeRequest,
                    VerifyForgetCodeEndpointHandler,
                    VerifyForgetCodeRequestValidator
                >()
                .RegisterEndpointResolver<
                    ResetPasswordRequest,
                    ResetPasswordEndpointHandler,
                    ResetPasswordValidator
                >()
                .RegisterAuthEndpointResolver<
                    AuthorizeRequest,
                    AuthorizeEndpointHandler,
                    AuthorizeRequestValidator
                >()
                .RegisterAuthEndpointResolver<
                    CreateAccountRequest,
                    CreateAccountEndpointHandler,
                    CreateAccountValidator
                >()
                .RegisterAuthEndpointResolver<
                    ChangeProfileRequest,
                    ChangeProfileEndpointHandler,
                    ChangeProfileRequestValidator
                >()
                .RegisterAuthEndpointResolver<
                    ChangePasswordRequest,
                    ChangePasswordEndpointHandler,
                    ChangePasswordRequestValidator
                >();
            return services;
        }

        public static IServiceCollection ConfigureAuth(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddAuthorizationBuilder().AddBasicPolicies().AddRegistrantPolicy();
            services.ConfigureJwtAuthentication(options =>
            {
                options.SecKey =
                    configuration["Authentication:SecKey"]
                    ?? throw new NullReferenceException("Couldn't find 'SecKey'.");
                options.Algorithms =
                [
                    configuration["Authentication:Algorithm"]
                        ?? throw new NullReferenceException("Couldn't find 'Algorithm'.")
                ];
            });
            return services;
        }

        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
            services.AddScoped<IRoleRespository, RoleRepository>();
            return services;
        }

        private static IServiceCollection AddDistributedCache(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString)
            );
            services.AddScoped<IAuthCodeCache, RedisAuthCache>();
            return services;
        }

        private static IServiceCollection RegisterEndpointResolver<TRequest, THandler, TValidator>(
            this IServiceCollection services
        )
            where TRequest : IRequest
            where THandler : class, IEndpointHandler<TRequest>
            where TValidator : class, IValidator<TRequest>
        {
            services.AddScoped<IEndpointHandler<TRequest>, THandler>();
            services.AddScoped<IValidator<TRequest>, TValidator>();
            return services;
        }

        private static IServiceCollection RegisterAuthEndpointResolver<
            TRequest,
            THandler,
            TValidator
        >(this IServiceCollection services)
            where TRequest : IRequest
            where THandler : class, IAuthEndpointHandler<TRequest>
            where TValidator : class, IValidator<TRequest>
        {
            services.AddScoped<IAuthEndpointHandler<TRequest>, THandler>();
            services.AddScoped<IValidator<TRequest>, TValidator>();
            return services;
        }
    }
}
