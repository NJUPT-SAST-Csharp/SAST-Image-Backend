using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct SendRegistrationCodeRequest
        : ICommandRequestObject<SendRegistrationCodeCommand>
    {
        public readonly string Email { get; init; }
    }
}
