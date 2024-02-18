using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct SendForgetCodeRequest(string email)
        : ICommandRequestObject<SendForgetCodeCommand>
    {
        public string Email { get; init; } = email;
    }
}
