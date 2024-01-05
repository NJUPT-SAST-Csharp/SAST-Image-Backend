using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator(IAuthCodeCache cache, IUserCheckRepository checker)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a.Code)
                .Cascade(CascadeMode.Stop)
                .InclusiveBetween(100000, 999999)
                .MustAsync(
                    (request, code, cancellationToken) =>
                        cache.VerifyCodeAsync(
                            CodeCacheKey.Registration,
                            request.Email,
                            code,
                            cancellationToken
                        )
                )
                .WithMessage("Incorrect code.");

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
        }
    }
}
