using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.UserEndpoints.ChangeProfile
{
    public sealed class ChangeProfileRequest : IRequest
    {
        public required string Nickname { get; init; }
        public required string Biography { get; init; }
        public string? Header { get; init; }
        public string? Avatar { get; init; }
    }
}
