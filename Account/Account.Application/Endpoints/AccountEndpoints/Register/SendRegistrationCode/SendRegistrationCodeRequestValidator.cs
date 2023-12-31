﻿using Account.Entity.UserEntity.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public class SendRegistrationCodeRequestValidator
        : AbstractValidator<SendRegistrationCodeRequest>
    {
        public SendRegistrationCodeRequestValidator(IUserCheckRepository checker)
        {
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
        }
    }
}
