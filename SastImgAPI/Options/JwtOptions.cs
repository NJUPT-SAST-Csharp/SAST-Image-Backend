#pragma warning disable CS1591
using Microsoft.IdentityModel.Tokens;

namespace SastImgAPI.Options
{
    public class JwtOptions
    {
        public required string SecKey { get; init; }
        public required int ExpireSeconds { get; init; }
    }
}
