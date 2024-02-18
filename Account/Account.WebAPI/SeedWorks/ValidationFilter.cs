using FluentValidation;
using Shared.Response.Builders;

namespace Account.WebAPI.SeedWorks
{
    internal sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator)
        : IEndpointFilter
        where TRequest : IBaseRequestObject
    {
        private readonly IValidator<TRequest> _validator = validator;

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
        )
        {
            var request =
                (TRequest)context.Arguments.First(a => a is TRequest)!
                ?? throw new NullReferenceException("Couldn't find the necessary request.");

            var result = await _validator.ValidateAsync(request);

            if (result.IsValid)
                return await next(context);

            var e = result.Errors.Find(
                e => e.ErrorCode == StatusCodes.Status409Conflict.ToString()
            );

            if (e is not null)
                return Responses.Conflict(
                    e.PropertyName,
                    e.AttemptedValue.ToString() ?? string.Empty
                );

            return Responses.ValidationFailure(result.ToDictionary());
        }
    }
}
