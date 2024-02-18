using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct VerifyForgetCodeRequest
        : ICommandRequestObject<VerifyForgetCodeCommand, VerifyForgetCodeDto>
    {
        public readonly string Email { get; init; }
        public readonly int Code { get; init; }
    }
}
