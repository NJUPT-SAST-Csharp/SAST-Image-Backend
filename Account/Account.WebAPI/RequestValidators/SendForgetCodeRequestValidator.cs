using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;

public sealed class SendForgetCodeRequestValidator : AbstractValidator<SendForgetCodeRequest>
{
    public SendForgetCodeRequestValidator()
    {
        RuleFor(r => r.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(EmailValidRule.MaxLength)
            .EmailAddress()
            .WithMessage("Invalid email");
    }
}
