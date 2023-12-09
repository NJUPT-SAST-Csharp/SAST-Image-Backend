using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Account.Register.Verify
{
    public sealed class VerifyRequestValidator : AbstractValidator<VerifyRequest>
    {
        public VerifyRequestValidator(IAuthCache cache, IUserCheckRepository checker)
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

            RuleFor(r => r.Code)
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
