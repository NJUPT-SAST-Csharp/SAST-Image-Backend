using Account.Application.SeedWorks;

namespace Account.Application.Account.Register.CreateAccount
{
    public sealed class CreateAccountRequest : IRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
        public required int Code { get; init; }
    }
}
