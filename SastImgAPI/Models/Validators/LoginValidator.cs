using FluentValidation;
using LanguageResources;
using Microsoft.Extensions.Localization;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator(IStringLocalizer<ValidationLanguage> localizer)
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(localizer["UsernameEmpty"])
                .Length(3, 20)
                .WithMessage(localizer["UsernameInvalid"]);
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["PasswordEmpty"])
                .Length(6, 20)
                .WithMessage(localizer["PasswordInvalid"]);
        }
    }
}
