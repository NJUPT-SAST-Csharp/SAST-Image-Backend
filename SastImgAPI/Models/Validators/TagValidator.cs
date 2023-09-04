using FluentValidation;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class TagValidator : AbstractValidator<TagDto>
    {
        public TagValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().Length(2, 6);
        }
    }
}
