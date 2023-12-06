using FluentValidation;

namespace Account.Application.Account.Register.SendCode
{
    public class SendCodeRequestValidator : AbstractValidator<SendCodeRequest>
    {
        public SendCodeRequestValidator()
        {
            RuleFor(r => r.Email).NotEmpty().MaximumLength(50).EmailAddress();
        }
    }
}
