using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeRequest : IRequest
    {
        public required string Email { get; init; }
        public required int Code { get; init; }
    }
}
