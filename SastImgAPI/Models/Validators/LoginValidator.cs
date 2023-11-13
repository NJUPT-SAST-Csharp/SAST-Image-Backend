using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().Length(3, 20);
            RuleFor(x => x.Password).NotEmpty().Length(6, 20);
        }
    }
}
