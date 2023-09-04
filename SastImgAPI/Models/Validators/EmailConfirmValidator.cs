using FluentValidation;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class EmailConfirmValidator : AbstractValidator<EmailConfirmDto>
    {
        public EmailConfirmValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format.");
            RuleFor(dto => dto.Token).NotEmpty().Length(6).WithMessage("Invalid token");
        }
    }
}
