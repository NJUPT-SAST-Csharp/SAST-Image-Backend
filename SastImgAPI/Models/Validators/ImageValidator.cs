using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class ImageValidator : AbstractValidator<ImageRequestDto>
    {
        public ImageValidator()
        {
            RuleFor(image => image.AlbumId).NotEmpty();
            RuleFor(image => image.ImageFile).NotEmpty();
            RuleFor(image => image.Title).NotEmpty().MaximumLength(12);
            RuleFor(image => image.Description).MaximumLength(100);
        }
    }
}
