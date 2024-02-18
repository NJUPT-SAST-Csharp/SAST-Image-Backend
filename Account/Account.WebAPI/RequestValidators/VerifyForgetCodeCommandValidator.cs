using Account.Application.Services;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeCommandValidator
        : AbstractValidator<VerifyForgetCodeCommand>
    {
        public VerifyForgetCodeCommandValidator(IAuthCodeCache cache)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .WithMessage("Invalid email");

            RuleFor(r => r.Code).Cascade(CascadeMode.Stop).InclusiveBetween(100000, 999999);
        }
    }
}
