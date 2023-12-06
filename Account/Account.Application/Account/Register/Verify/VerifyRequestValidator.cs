using FluentValidation;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyRequestValidator : AbstractValidator<VerifyRequest>
    {
        public VerifyRequestValidator()
        {
            RuleFor(r => r.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(r => r.Code).InclusiveBetween(100000, 999999);
        }
    }
}
