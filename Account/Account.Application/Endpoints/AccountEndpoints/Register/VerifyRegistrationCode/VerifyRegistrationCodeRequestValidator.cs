using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeRequestValidator
        : AbstractValidator<VerifyRegistrationCodeRequest>
    {
        public VerifyRegistrationCodeRequestValidator(
            IAuthCodeCache cache,
            IUserCheckRepository checker
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .MustAsync(
                    async (email, cancellationToken) =>
                        await checker.CheckEmailExistenceAsync(email, cancellationToken) == false
                )
                .WithErrorCode(StatusCodes.Status409Conflict.ToString())
                .WithMessage("Duplicated email.");

            RuleFor(r => r.Code)
                .Cascade(CascadeMode.Stop)
                .InclusiveBetween(100000, 999999)
                .MustAsync(
                    (v, code, cancellationToken) =>
                        cache.VerifyCodeAsync(
                            CodeCacheKey.Registration,
                            v.Email,
                            code,
                            cancellationToken
                        )
                )
                .WithMessage("Incorrect code.");
        }
    }
}
