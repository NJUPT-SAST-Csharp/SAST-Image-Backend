using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeRequest : IRequest
    {
        public required string Email { get; init; }
    }
}
