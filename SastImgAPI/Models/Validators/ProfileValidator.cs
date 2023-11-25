using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class ProfileValidator : AbstractValidator<ProfileRequestDto>
    {
        public ProfileValidator()
        {
            RuleFor(dto => dto.Nickname).NotEmpty().Length(2, 12);
            RuleFor(dto => dto.Biography)
                .Length(0, 100)
                .Unless(dto => string.IsNullOrEmpty(dto.Biography));
        }
    }
}
