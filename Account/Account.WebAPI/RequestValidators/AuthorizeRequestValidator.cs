using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators;

internal sealed class AuthorizeRequestValidator : AbstractValidator<AuthorizeRequest>
{
    public AuthorizeRequestValidator()
    {
        RuleFor(r => r.UserId).GreaterThan(0).WithMessage("Invalid user id.");
    }
}
