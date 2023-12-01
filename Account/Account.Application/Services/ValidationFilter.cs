using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Services
{
    public sealed class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
        where T : class
    {
        private readonly IValidator<T> _validator = validator;

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
        )
        {
            var request =
                context.Arguments.First(a => a is T) as T ?? throw new NullReferenceException();
            var result = await _validator.ValidateAsync(request);
            if (result.IsValid)
                return await next(context);
            return Responses.ValidationFailure(result.ToDictionary());
        }
    }
}
