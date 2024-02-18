using Account.Application.Services;
using Account.Domain.UserEntity.Rules;
using Account.Domain.UserEntity.Services;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class VerifyRegistrationCodeRequestValidator
        : AbstractValidator<VerifyRegistrationCodeRequest>
    {
        public VerifyRegistrationCodeRequestValidator(
            IAuthCodeCache cache,
            IUserUniquenessChecker checker
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(EmailValidRule.MaxLength)
                .EmailAddress()
                .WithMessage("Invalid email.");
        }
    }
}
