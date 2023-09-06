using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class EmailConfirmValidator : AbstractValidator<EmailConfirmRequestDto>
    {
        public EmailConfirmValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format.");
            RuleFor(dto => dto.Token).NotEmpty().Length(6).WithMessage("Invalid token");
        }
    }
}
