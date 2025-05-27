using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators;

public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(r => r.NewPassword)
            .NotEmpty()
            .Length(PasswordValidRule.MinLength, PasswordValidRule.MaxLength);
    }
}
