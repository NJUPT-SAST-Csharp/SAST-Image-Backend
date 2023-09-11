using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class ImageValidator : AbstractValidator<ImageRequestDto>
    {
        public ImageValidator()
        {
            RuleFor(image => image.AlbumId).NotEmpty().Length(11);
            RuleFor(image => image.ImageFile).NotEmpty();
            RuleFor(image => image.Title).NotEmpty().MaximumLength(12);
            RuleFor(image => image.Description).MaximumLength(100);
            RuleFor(x => x.Category).Length(11).When(x => !string.IsNullOrEmpty(x.Category));
            RuleFor(x => x.Category)
                .Must(x => IsLongBase64(x))
                .WithMessage("The category isn't valid base64 string.");
            RuleForEach(image => image.Tags)
                .Length(11)
                .Must(x => IsLongBase64(x))
                .WithMessage("The tag isn't valid base64 string.")
                .When(image => image.Tags is not null && image.Tags.Any());
        }

        private static bool IsLongBase64(string s)
        {
            try
            {
                var bytes = WebEncoders.Base64UrlDecode(s);
                BitConverter.ToInt64(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
