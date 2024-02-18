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
                .MaximumLength(50)
                .EmailAddress()
                .WithMessage("Invalid email");
        }
    }
}
