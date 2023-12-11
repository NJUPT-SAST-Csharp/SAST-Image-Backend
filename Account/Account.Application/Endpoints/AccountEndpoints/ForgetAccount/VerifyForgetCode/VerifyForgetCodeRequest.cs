using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeRequest : IRequest
    {
        public required int Code { get; init; }
        public required string Email { get; init; }
    }
}
