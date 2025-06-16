using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Account.Domain.UserEntity;
using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Account.Infrastructure.ApplicationServices;

internal sealed class JwtProvider(IConfiguration configuration) : IJwtTokenGenerator
{
    private readonly string secKey =
        configuration.GetRequiredSection(nameof(Auth)).GetValue<string?>("SecKey")
        ?? throw new NullReferenceException("Couldn't find 'SecKey' from configuration.");
    private readonly long expires = configuration
        .GetRequiredSection(nameof(Auth))
        .GetValue<long>("Expires");

    public JwtToken Issue(User user)
    {
        var claims = new List<Claim>() { new(nameof(UserId), user.Id.Value.ToString()) };
        claims.AddRange(user.Roles.Select(r => new Claim(nameof(Role), ((int)r).ToString())));
        string token = GenerateJwtByClaims(claims);

        return new(token, expires);
    }

    private string GenerateJwtByClaims(ICollection<Claim> claims, TimeSpan? expiration = null)
    {
        if (expires <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expiration),
                "Expire time must be greater than 0."
            );
        }

        var expireTime = DateTime.UtcNow.Add(expiration ?? TimeSpan.FromSeconds(expires));
        byte[] secBytes = Encoding.Default.GetBytes(secKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(secBytes),
            SecurityAlgorithms.HmacSha256
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
