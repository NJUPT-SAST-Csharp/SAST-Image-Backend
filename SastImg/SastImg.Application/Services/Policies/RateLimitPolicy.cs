namespace SastImg.Application.Services.Policies
{
    public static class RateLimitPolicy
    {
        public const string Global = "Global";
        public const string Authenticated = "Authenticated";
        public const string Anonymous = "Anonymous";
    }
}
