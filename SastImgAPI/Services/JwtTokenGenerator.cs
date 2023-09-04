using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SastImgAPI.Models.Identity;
using SastImgAPI.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SastImgAPI.Services
{
    public class JwtTokenGenerator
    {
        private readonly JwtOptions _option;
        private readonly UserManager<User> _userManager;

        public JwtTokenGenerator(IOptionsSnapshot<JwtOptions> option, UserManager<User> userManager)
        {
            _option = option.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtByUserAsync(User user)
        {
            List<Claim> claims =
                new() { new Claim("id", user.Id.ToString()), new Claim("sub", user.UserName!), };
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim("role", role));
            }
            return GenerateJwtByClaims(claims);
        }

        public string GenerateJwtByClaims(ICollection<Claim> claims) =>
            GenerateJwtByClaims(claims, TimeSpan.FromSeconds(_option.ExpireSeconds));

        public string GenerateJwtByClaims(ICollection<Claim> claims, TimeSpan expiration)
        {
            DateTime expireTime = DateTime.Now.Add(expiration);
            byte[] secBytes = Encoding.UTF8.GetBytes(_option.SecKey);
            var secKey = new SymmetricSecurityKey(secBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
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
