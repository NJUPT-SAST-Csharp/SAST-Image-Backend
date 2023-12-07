using Account.Application.Services;
using FluentValidation;

namespace Account.Application.Account.Register.CreateAccount
{
    public sealed class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator(IAuthCache cache)
        {
            RuleFor(a => a.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(a => a.Code).InclusiveBetween(100000, 999999);
            RuleFor(a => a.Username).NotEmpty().MaximumLength(20);
            RuleFor(a => a.Password).NotEmpty().Length(6, 30);

            RuleFor(r => r.Code)
                .MustAsync(
                    (v, code, cancellationToken) =>
                        cache.VerifyCodeAsync(v.Email, code.ToString(), cancellationToken)
                )
                .WithMessage("Incorrect code.");
        }
    }
}
