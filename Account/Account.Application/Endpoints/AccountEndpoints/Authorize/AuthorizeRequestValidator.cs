using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeRequestValidator : AbstractValidator<AuthorizeRequest>
    {
        public AuthorizeRequestValidator()
        {
            RuleFor(r => r.UserId);
            RuleFor(r => r.RoleId).LessThan(100).WithMessage("Invalid role id.");
        }
    }
}
