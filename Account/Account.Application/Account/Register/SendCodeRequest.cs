using Account.Application.SeedWorks;

namespace Account.Application.Account.Register
{
    public sealed class SendCodeRequest : IRequest
    {
        public string Email { get; init; } = string.Empty;
    }
}
