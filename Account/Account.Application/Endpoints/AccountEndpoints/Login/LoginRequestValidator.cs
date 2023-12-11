using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().Length(0, 20);
            RuleFor(x => x.Password).NotEmpty().Length(6, 20);
        }
    }
}
