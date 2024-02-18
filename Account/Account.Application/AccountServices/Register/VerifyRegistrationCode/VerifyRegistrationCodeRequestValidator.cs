using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeRequestValidator
        : AbstractValidator<VerifyRegistrationCodeCommand>
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
                .MaximumLength(50)
                .EmailAddress()
                .WithMessage("Invalid email.");
        }
    }
}
