using FluentValidation;

namespace Account.Application.Endpoints.UserEndpoints.Query
{
    public sealed class QueryUserRequestValidator : AbstractValidator<QueryUserRequest>
    {
        public QueryUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .When(x => x.UserId.HasValue == false || x.UserId == 0)
                .WithMessage("UserId or Username must be provided.");
        }
    }
}
