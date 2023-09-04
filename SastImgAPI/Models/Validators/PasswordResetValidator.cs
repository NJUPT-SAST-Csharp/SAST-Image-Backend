using FluentValidation;
using Microsoft.Extensions.Localization;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class PasswordResetValidator : AbstractValidator<PasswordResetDto>
    {
        public PasswordResetValidator()
        {
            RuleFor(x => x.NewPassword).Length(6, 20).Equal(x => x.ConfirmPassword);
        }
    }
}
