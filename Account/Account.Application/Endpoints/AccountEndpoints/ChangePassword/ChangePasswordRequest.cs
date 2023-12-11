using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword
{
    public sealed class ChangePasswordRequest : IRequest
    {
        public required string FormerPassword { get; init; }
        public required string NewPassword { get; init; }
    }
}
