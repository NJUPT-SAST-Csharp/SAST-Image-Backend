using FluentValidation;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class AlbumValidation : AbstractValidator<AlbumRequestDto>
    {
        public AlbumValidation()
        {
            RuleFor(dto => dto.Accessibility).IsInEnum();
            RuleFor(dto => dto.Name).NotEmpty().Length(1, 20);
            RuleFor(dto => dto.Description).Length(0, 100);
        }
    }
}
