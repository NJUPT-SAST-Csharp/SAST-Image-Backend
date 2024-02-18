using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    internal sealed class AuthorizeCommandValidator : AbstractValidator<AuthorizeRequest>
    {
        public AuthorizeCommandValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0).WithMessage("Invalid user id.");
            RuleFor(r => r.RoleId).ExclusiveBetween(0, 100).WithMessage("Invalid role id.");
        }
    }
}
