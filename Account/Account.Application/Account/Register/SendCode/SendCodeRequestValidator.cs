using Account.Entity.UserEntity.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Account.Register.SendCode
{
    public class SendCodeRequestValidator : AbstractValidator<SendCodeRequest>
    {
        public SendCodeRequestValidator(IUserCheckRepository checker)
        {
            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .MustAsync(
                    async (_, email, cancellationToken) =>
                        await checker.CheckEmailExistenceAsync(email, cancellationToken) == false
                )
                .WithErrorCode(StatusCodes.Status409Conflict.ToString())
                .WithMessage("Duplicated email.");
        }
    }
}
