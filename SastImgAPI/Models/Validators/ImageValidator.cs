using FluentValidation;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Models.Validators
{
    public class ImageValidator : AbstractValidator<ImageDto>
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
