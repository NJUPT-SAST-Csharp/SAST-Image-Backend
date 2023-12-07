using Account.Application.Services;
using FluentValidation;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyRequestValidator : AbstractValidator<VerifyRequest>
    {
        public VerifyRequestValidator(IAuthCache cache)
        {
            RuleFor(r => r.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(r => r.Code).InclusiveBetween(100000, 999999);

            RuleFor(r => r.Code)
                .MustAsync(
                    (v, code, cancellationToken) =>
                        cache.VerifyCodeAsync(v.Email, code.ToString(), cancellationToken)
                )
                .WithMessage("Incorrect code.");
        }
    }
}
