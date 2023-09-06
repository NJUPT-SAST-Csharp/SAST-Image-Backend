using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class TagValidator : AbstractValidator<TagRequestDto>
    {
        public TagValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().Length(2, 6);
        }
    }
}
