using Account.WebAPI.Requests;
using FluentValidation;

namespace Account.WebAPI.RequestValidators
{
    public sealed class GetUserInfoRequestValidator : AbstractValidator<GetUserInfoRequest>
    {
        public GetUserInfoRequestValidator() { }
    }
}
