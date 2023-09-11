using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class EmailSendValidator : AbstractValidator<EmailSendRequestDto>
    {
        public EmailSendValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty().EmailAddress();
        }
    }
}
