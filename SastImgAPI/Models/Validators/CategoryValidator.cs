using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 10);
            RuleFor(x => x.Description).Length(0, 100);
        }
    }
}
