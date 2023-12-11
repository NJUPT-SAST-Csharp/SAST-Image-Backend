using Account.Application.Services;
using Auth.Authorization;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace Account.Infrastructure.Services
{
    internal sealed class JwtProvider(IConfiguration configuration) : IJwtProvider
    {
        private readonly JwtTokenGenerator _generator = new(configuration);

        public string GetLoginJwt(string userId, string username)
        {
            return _generator.GenerateJwtByClaims(
                [
                    new("Id", userId),
                    new("Username", username),
                    new("Roles", AuthorizationRoles.User)
                ]
            );
        }

        public string GetRegistrantJwt(string email)
        {
            return _generator.GenerateJwtByClaims(
                [new("Email", email), new("Roles", AuthorizationRoles.Registrant)],
                TimeSpan.FromMinutes(30)
            );
        }
    }
}
