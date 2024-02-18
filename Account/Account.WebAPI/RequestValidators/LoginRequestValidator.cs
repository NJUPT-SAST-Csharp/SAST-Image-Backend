using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(UsernameValidRule.MinLength, UsernameValidRule.MaxLength);
            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(PasswordValidRule.MinLength, PasswordValidRule.MaxLength);
        }
    }
}
