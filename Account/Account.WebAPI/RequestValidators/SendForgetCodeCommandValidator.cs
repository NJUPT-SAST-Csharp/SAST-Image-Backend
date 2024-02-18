using Account.Domain.UserEntity.Rules;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeCommandValidator : AbstractValidator<SendForgetCodeCommand>
    {
        public SendForgetCodeCommandValidator()
        {
            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(EmailValidRule.MaxLength)
                .EmailAddress()
                .WithMessage("Invalid email");
        }
    }
}
