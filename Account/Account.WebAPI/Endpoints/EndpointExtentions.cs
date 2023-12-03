using Account.Application.SeedWorks;
using Account.Application.Services;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointExtentions
    {
        public static RouteHandlerBuilder AddValidator<TRequest>(this RouteHandlerBuilder builder)
            where TRequest : class, IRequest
        {
            builder.AddEndpointFilter<ValidationFilter<TRequest>>();
            return builder;
        }
    }
}
