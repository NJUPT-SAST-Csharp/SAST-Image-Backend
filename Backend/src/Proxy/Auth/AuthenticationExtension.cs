using System.Text;
using Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Proxy.Auth;

public static class AuthenticationExtension
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection services, string secKey)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    RoleClaimType = Requester.RolesClaimType,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey)),
                    ValidateLifetime = true,
                    LifetimeValidator = (notbefore, expires, _, _) =>
                    {
                        return DateTime.UtcNow > (notbefore ?? DateTime.MinValue)
                            && DateTime.UtcNow < (expires ?? DateTime.MaxValue);
                    },
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                };
            });
        return services;
    }
}
