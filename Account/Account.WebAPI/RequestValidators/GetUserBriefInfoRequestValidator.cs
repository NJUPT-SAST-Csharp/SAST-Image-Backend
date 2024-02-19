using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class GetUserBriefInfoRequestValidator
        : AbstractValidator<GetUserBriefInfoRequest>
    {
        public GetUserBriefInfoRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(UsernameValidRule.MinLength, UsernameValidRule.MaxLength);
        }
    }
}
