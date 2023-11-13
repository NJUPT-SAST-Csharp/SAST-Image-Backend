using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Nickname).NotEmpty().Length(2, 12).WithMessage("Invalid nickname.");
            RuleFor(x => x.Username).NotEmpty().Length(2, 12);
            RuleFor(x => x.Password).NotEmpty().Length(6, 20);
        }
    }
}
