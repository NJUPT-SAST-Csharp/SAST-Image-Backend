using FluentValidation;

namespace Account.Application.Endpoints.UserEndpoints.ChangeProfile
{
    public sealed class ChangeProfileRequestValidator : AbstractValidator<ChangeProfileRequest>
    {
        public ChangeProfileRequestValidator()
        {
            RuleFor(r => r.Nickname).NotEmpty().MaximumLength(20);
            RuleFor(r => r.Biography).NotEmpty().MaximumLength(100);
            RuleFor(r => r.Header).MaximumLength(300);
            RuleFor(r => r.Avatar).MaximumLength(300);
        }
    }
}
