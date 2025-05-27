using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators;

public sealed class UpdateAvatarRequestValidator : AbstractValidator<UpdateAvatarRequest>
{
    public UpdateAvatarRequestValidator()
    {
        RuleFor(x => x.AvatarFile)
            .Must(AvatarFileValidationRule)
            .When(x => x.AvatarFile is not null);
    }

    private bool AvatarFileValidationRule(IFormFile file)
    {
        return file.Length < 1024 * 1024 * 10;
    }
}
