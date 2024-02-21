using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class UpdateHeaderRequestValidator : AbstractValidator<UpdateHeaderRequest>
    {
        public UpdateHeaderRequestValidator()
        {
            RuleFor(x => x.HeaderFile)
                .Must(HeaderFileValidationRule)
                .When(x => x.HeaderFile is not null);
        }

        private bool HeaderFileValidationRule(IFormFile file)
        {
            return file.Length < 1024 * 1024 * 20;
        }
    }
}
