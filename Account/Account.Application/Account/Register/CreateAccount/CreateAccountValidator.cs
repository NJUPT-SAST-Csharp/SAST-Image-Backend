using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Account.Register.CreateAccount
{
    public sealed class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator(IAuthCache cache, IUserCheckRepository checker)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a.Password).NotEmpty().Length(6, 30);

            RuleFor(a => a.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(20)
                .MustAsync(
                    async (_, username, cancellationToken) =>
                        await checker.CheckUsernameExistenceAsync(username, cancellationToken)
                        == false
                )
                .WithErrorCode(StatusCodes.Status409Conflict.ToString())
                .WithMessage("Duplicated username.");

            RuleFor(a => a.Email)
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

            RuleFor(a => a.Code)
                .Cascade(CascadeMode.Stop)
                .InclusiveBetween(100000, 999999)
                .MustAsync(
                    (v, code, cancellationToken) =>
                        cache.VerifyCodeAsync(v.Email, code.ToString(), cancellationToken)
                )
                .WithMessage("Incorrect code.");
        }
    }
}
