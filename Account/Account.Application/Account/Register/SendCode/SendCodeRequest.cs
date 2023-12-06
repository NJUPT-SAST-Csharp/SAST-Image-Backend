using Account.Application.SeedWorks;

namespace Account.Application.Account.Register.SendCode
{
    public sealed class SendCodeRequest : IRequest
    {
        public required string Email { get; init; }
    }
}
