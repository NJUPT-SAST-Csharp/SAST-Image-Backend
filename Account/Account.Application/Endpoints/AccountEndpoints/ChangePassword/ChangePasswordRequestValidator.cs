using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword
{
    public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(r => r.NewPassword).NotEmpty().Length(6, 20);
            RuleFor(r => r.FormerPassword).NotEmpty().Length(6, 20);
        }
    }
}
