using Account.Domain.UserEntity.Rules;
using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class GetUserDetailedInfoRequestValidator
        : AbstractValidator<GetUserDetailedInfoRequest>
    {
        public GetUserDetailedInfoRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(UsernameValidRule.MinLength, UsernameValidRule.MaxLength);
        }
    }
}
