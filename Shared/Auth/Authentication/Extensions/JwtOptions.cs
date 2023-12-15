namespace Auth.Authentication.Extensions
{
    public sealed class JwtOptions
    {
        public string SecKey { get; set; } = string.Empty;
        public string[] Algorithms { get; set; } = [];
    }
}
