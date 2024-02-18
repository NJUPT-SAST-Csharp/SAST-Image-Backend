using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeCommandValidator : AbstractValidator<AuthorizeCommand>
    {
        public AuthorizeCommandValidator()
        {
            RuleFor(r => r.UserId.Value).GreaterThan(0).WithMessage("Invalid user id.");
            RuleFor(r => r.RoleId.Value).ExclusiveBetween(0, 100).WithMessage("Invalid role id.");
        }
    }
}
