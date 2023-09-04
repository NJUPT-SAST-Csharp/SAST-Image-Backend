using FluentValidation;
using LanguageResources;
using Microsoft.Extensions.Localization;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class PasswordResetValidator : AbstractValidator<PasswordResetDto>
    {
        public PasswordResetValidator(IStringLocalizer<ValidationLanguage> localizer)
        {
            RuleFor(x => x.NewPassword)
                .Length(6, 20)
                .WithMessage(localizer["PasswordInvalid"])
                .Equal(x => x.ConfirmPassword)
                .WithMessage(localizer["ConfirmPasswordInvalid"]);
        }
    }
}
