using Account.Domain.UserEntity.Rules;
using Account.Domain.UserEntity.Services;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators;

public class SendRegistrationCodeRequestValidator : AbstractValidator<SendRegistrationCodeRequest>
{
    public SendRegistrationCodeRequestValidator(IUserUniquenessChecker checker)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(EmailValidRule.MaxLength)
            .EmailAddress()
            .WithMessage("Invalid email.");

        RuleFor(r => r.Email)
            .MustAsync(
                async (email, cancellationToken) =>
                    await checker.CheckEmailExistenceAsync(email, cancellationToken) == false
            )
            .WithErrorCode(StatusCodes.Status409Conflict.ToString())
            .WithMessage("Duplicated email.");
    }
}
