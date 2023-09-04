using FluentValidation;
using Microsoft.Extensions.Localization;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().Length(3, 20);
            RuleFor(x => x.Password).NotEmpty().Length(6, 20);
        }
    }
}
