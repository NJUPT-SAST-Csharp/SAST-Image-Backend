using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Utilities
{
    public sealed class JwtTokenGenerator(string algorithm, string secKey, int expires)
    {
        private readonly string _algorithm = algorithm;
        private readonly string _secKey = secKey;
        private readonly int _expires = expires;

        public JwtTokenGenerator(IConfiguration configuration)
            : this(
                configuration["Authentication:Algorithm"]
                    ?? throw new NullReferenceException(
                        "Couldn't find 'Algorithm' from configuration."
                    ),
                configuration["Authentication:SecKey"]
                    ?? throw new NullReferenceException(
                        "Couldn't find 'SecKey' from configuration."
                    ),
                configuration.GetRequiredSection("Authentication").GetValue<int>("Expires")
            ) { }

        public string GenerateJwtByClaims(ICollection<Claim> claims, TimeSpan? expiration = null)
        {
            DateTime expireTime = DateTime.UtcNow.Add(expiration ?? TimeSpan.FromSeconds(_expires));
            byte[] secBytes = Encoding.Default.GetBytes(_secKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(secBytes),
                _algorithm
            );

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: expireTime,
                signingCredentials: credentials
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }
    }
}
