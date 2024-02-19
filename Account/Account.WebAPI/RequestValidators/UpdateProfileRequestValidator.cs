using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.Nickname)
                .NotEmpty()
                .Length(NicknameLengthRule.MinLength, NicknameLengthRule.MaxLength);

            RuleFor(x => x.Biography).MaximumLength(BiographyValidRule.MaxLength);
        }
    }
}
