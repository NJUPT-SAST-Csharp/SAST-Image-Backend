using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct ChangePasswordRequest : ICommandRequestObject<ChangePasswordCommand>
    {
        public readonly string NewPassword { get; init; }
    }
}
