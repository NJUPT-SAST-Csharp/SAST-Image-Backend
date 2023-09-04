using FluentValidation;
using LanguageResources;
using Microsoft.Extensions.Localization;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator(IStringLocalizer<ValidationLanguage> localizer)
        {
            RuleFor(x => x.Nickname).NotEmpty().Length(2, 12).WithMessage("Invalid nickname.");
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(localizer["UsernameEmpty"])
                .Length(2, 12)
                .WithMessage(localizer["UsernameInvalid"]);
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["PasswordEmpty"])
                .Length(6, 20)
                .WithMessage(localizer["PasswordInvalid"]);
        }
    }
}
