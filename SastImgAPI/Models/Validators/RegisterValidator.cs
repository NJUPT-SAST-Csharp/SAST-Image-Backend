using FluentValidation;
using Microsoft.Extensions.Localization;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Nickname).NotEmpty().Length(2, 12).WithMessage("Invalid nickname.");
            RuleFor(x => x.Username).NotEmpty().Length(2, 12);
            RuleFor(x => x.Password).NotEmpty().Length(6, 20);
        }
    }
}
