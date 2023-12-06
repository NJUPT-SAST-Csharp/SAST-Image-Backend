using Account.Application.SeedWorks;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyRequest : IRequest
    {
        public required string Email { get; init; }
        public required int Code { get; init; }
    }
}
