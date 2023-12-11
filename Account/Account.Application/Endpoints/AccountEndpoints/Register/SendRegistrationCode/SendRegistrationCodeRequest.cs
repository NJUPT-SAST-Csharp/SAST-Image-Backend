using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public sealed class SendRegistrationCodeRequest : IRequest
    {
        public required string Email { get; init; }
    }
}
