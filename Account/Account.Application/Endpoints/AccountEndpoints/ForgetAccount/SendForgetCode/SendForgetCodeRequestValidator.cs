using Account.Entity.UserEntity.Repositories;
using FluentValidation;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeRequestValidator : AbstractValidator<SendForgetCodeRequest>
    {
        public SendForgetCodeRequestValidator(IUserCheckRepository checker)
        {
            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress()
                .MustAsync(checker.CheckEmailExistenceAsync)
                .WithMessage("Invalid email");
        }
    }
}
