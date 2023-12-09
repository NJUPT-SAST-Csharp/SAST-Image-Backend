using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

        public static RouteHandlerBuilder AddPost<TRequest>(
            this RouteGroupBuilder builder,
            string route
        )
            where TRequest : class, IRequest
        {
            return builder
                .MapPost(
                    route,
                    ([FromServices] IEndpointHandler<TRequest> handler, TRequest request) =>
                        handler.Handle(request)
                )
                .AddValidator<TRequest>();
        }

        public static RouteHandlerBuilder AddAuthPost<TRequest>(
            this RouteGroupBuilder builder,
            string route,
            Func<ClaimsPrincipal, TRequest> middle
        )
            where TRequest : class, IRequest
        {
            return builder
                .MapPost(
                    route,
                    (
                        [FromServices] IEndpointHandler<TRequest> handler,
                        TRequest request,
                        ClaimsPrincipal user
                    ) => handler.Handle(request)
                )
                .AddValidator<TRequest>();
        }
    }
}
