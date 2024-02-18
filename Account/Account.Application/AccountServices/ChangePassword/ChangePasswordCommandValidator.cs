using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword
{
    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(r => r.NewPassword).NotEmpty().Length(6, 20);
        }
    }
}
