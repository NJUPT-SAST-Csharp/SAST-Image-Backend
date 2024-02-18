using Account.Domain.UserEntity.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public class SendRegistrationCodeRequestValidator
        : AbstractValidator<SendRegistrationCodeCommand>
    {
        public SendRegistrationCodeRequestValidator(IUserUniquenessChecker checker)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
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
}
