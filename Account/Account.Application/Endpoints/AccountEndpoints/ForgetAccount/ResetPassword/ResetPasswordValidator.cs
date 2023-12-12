using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword
{
    public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator(IAuthCache cache, IUserCheckRepository checker)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Password).NotEmpty().MaximumLength(20);

            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .MustAsync(checker.CheckEmailExistenceAsync)
                .WithMessage("Invalid email");

            RuleFor(r => r.Code)
                .Cascade(CascadeMode.Stop)
                .InclusiveBetween(100000, 999999)
                .MustAsync(
                    (request, code, cancellationToken) =>
                        cache.VerifyCodeAsync(
                            CacheKeys.ForgetAccount,
                            request.Email,
                            code,
                            cancellationToken
                        )
                )
                .WithMessage("Invalid code");
        }
    }
}
