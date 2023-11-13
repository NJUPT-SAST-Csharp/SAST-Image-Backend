using FluentValidation;
using SastImgAPI.Models.RequestDtos;

namespace SastImgAPI.Models.Validators
{
    public class PasswordResetValidator : AbstractValidator<PasswordResetRequestDto>
    {
        public PasswordResetValidator()
        {
            RuleFor(x => x.NewPassword).Length(6, 20).Equal(x => x.ConfirmPassword);
        }
    }
}
