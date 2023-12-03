using FluentValidation;

namespace Account.Application.Account.Register
{
    public class SendCodeRequestValidator : AbstractValidator<SendCodeRequest>
    {
        public SendCodeRequestValidator()
        {
            RuleFor(r => r.Email).NotEmpty().MaximumLength(50).EmailAddress();
        }
    }
}
