using System.Security.Claims;
using Account.Application.Services;
using Account.Domain.UserEntity;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace Account.Infrastructure.ApplicationServices
{
    internal sealed class JwtProvider(IConfiguration configuration) : IJwtProvider
    {
        private readonly JwtTokenGenerator _generator = new(configuration);

        public string GetLoginJwt(UserId userId, string username, IEnumerable<Role> roles)
        {
            var claims = new List<Claim>()
            {
                new("Id", userId.Value.ToString()),
                new("Username", username)
            };
            claims.AddRange(roles.Select(r => new Claim("Roles", r.ToString())));
            return _generator.GenerateJwtByClaims(claims);
        }
    }
}
