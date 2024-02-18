using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct VerifyRegistrationCodeRequest
        : ICommandRequestObject<VerifyRegistrationCodeCommand, VerifyRegistrationCodeDto>
    {
        public readonly string Email { get; init; }
        public readonly int Code { get; init; }
    }
}
