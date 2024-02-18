using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator(IAuthCodeCache cache, IUserUniquenessChecker checker)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .WithMessage("Invalid email");

            RuleFor(a => a.Password).NotEmpty().Length(6, 30);

            RuleFor(a => a.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Invalid username");

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
                .MustAsync(
                    async (email, cancellationToken) =>
                        await checker.CheckEmailExistenceAsync(email, cancellationToken) == false
                )
                .WithErrorCode(StatusCodes.Status409Conflict.ToString())
                .WithMessage("Duplicated email.");

            RuleFor(a => a.Username)
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
