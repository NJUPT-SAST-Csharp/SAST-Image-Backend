using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Authentication.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection ConfigureJwtAuthentication(
            this IServiceCollection services,
            JwtOptions jwtOptions
        )
        {
            return Configure(services, jwtOptions);
        }

        public static IServiceCollection ConfigureJwtAuthentication(
            this IServiceCollection services,
            Action<JwtOptions> configureOptions
        )
        {
            JwtOptions options = new();
            configureOptions(options);
            return Configure(services, options);
        }

        private static IServiceCollection Configure(
            IServiceCollection services,
            JwtOptions jwtOptions
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var secKey = jwtOptions.SecKey;

                    options.TokenValidationParameters = new()
                    {
                        NameClaimType = "Username",
                        RoleClaimType = "Roles",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.Default.GetBytes(secKey)
                        ),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        LifetimeValidator = (notbefore, expires, _, _) =>
                        {
                            return DateTime.UtcNow > (notbefore ?? DateTime.MinValue)
                                && DateTime.UtcNow < (expires ?? DateTime.MaxValue);
                        },
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAlgorithms = jwtOptions.Algorithms
                    };
                });
            return services;
        }
    }
}
