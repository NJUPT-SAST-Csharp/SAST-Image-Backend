using Account.Application.Services;
using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeCommandValidator
        : AbstractValidator<VerifyForgetCodeRequest>
    {
        public VerifyForgetCodeCommandValidator(IAuthCodeCache cache)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email)
                .NotEmpty()
                .MaximumLength(EmailValidRule.MaxLength)
                .EmailAddress()
                .WithMessage("Invalid email");

            RuleFor(r => r)
                .MustAsync(
                    async (request, cancellationToken) =>
                    {
                        var result = await cache.VerifyCodeAsync(
                            CodeCacheKey.ForgetAccount,
                            request.Email,
                            request.Code,
                            cancellationToken
                        );
                        return result;
                    }
                )
                .WithMessage("Invalid code");

            RuleFor(r => r.Code).Cascade(CascadeMode.Stop).InclusiveBetween(100000, 999999);
        }
    }
}
