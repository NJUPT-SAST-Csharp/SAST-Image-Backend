using System.Security.Claims;
using Account.Application.Services;
using Auth.Authorization;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace Account.Infrastructure.Services
{
    internal sealed class JwtProvider(IConfiguration configuration) : IJwtProvider
    {
        private readonly JwtTokenGenerator _generator = new(configuration);

        public string GetLoginJwt(string userId, string username, IEnumerable<string> roles)
        {
            var claims = new List<Claim>() { new("Id", userId), new("Username", username) };
            claims.AddRange(roles.Select(r => new Claim("Roles", r)));
            return _generator.GenerateJwtByClaims(claims);
        }

        public string GetRegistrantJwt(string email)
        {
            return _generator.GenerateJwtByClaims(
                [new("Email", email), new("Roles", AuthorizationRole.Registrant.ToString())],
                TimeSpan.FromMinutes(30)
            );
        }
    }
}
