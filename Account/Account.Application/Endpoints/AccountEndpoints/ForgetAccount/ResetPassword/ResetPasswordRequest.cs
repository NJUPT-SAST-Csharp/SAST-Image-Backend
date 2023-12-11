using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword
{
    public sealed class ResetPasswordRequest : IRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required int Code { get; init; }
    }
}
