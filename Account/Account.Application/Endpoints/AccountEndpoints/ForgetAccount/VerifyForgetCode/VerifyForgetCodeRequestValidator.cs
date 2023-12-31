﻿using Account.Application.Services;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeRequestValidator
        : AbstractValidator<VerifyForgetCodeRequest>
    {
        public VerifyForgetCodeRequestValidator(IAuthCodeCache cache)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Email).NotEmpty().MaximumLength(50).EmailAddress();

            RuleFor(r => r.Code)
                .Cascade(CascadeMode.Stop)
                .InclusiveBetween(100000, 999999)
                .MustAsync(
                    (request, code, cancellationToken) =>
                        cache.VerifyCodeAsync(
                            CodeCacheKey.ForgetAccount,
                            request.Email,
                            code,
                            cancellationToken
                        )
                )
                .WithMessage("Invalid email");
        }
    }
}
