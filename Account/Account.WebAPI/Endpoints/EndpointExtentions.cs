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

        public static RouteHandlerBuilder AddPost<TRequest>(
            this RouteGroupBuilder builder,
            string route,
            string policy
        )
            where TRequest : class, IRequest
        {
            return builder
                .MapPost(
                    route,
                    (
                        [FromServices] IAuthEndpointHandler<TRequest> handler,
                        TRequest request,
                        ClaimsPrincipal user
                    ) => handler.Handle(request, user)
                )
                .AddValidator<TRequest>()
                .RequireAuthorization(policy);
        }

        public static RouteHandlerBuilder AddPut<TRequest>(
            this RouteGroupBuilder builder,
            string route,
            string policy
        )
            where TRequest : class, IRequest
        {
            return builder
                .MapPut(
                    route,
                    (
                        [FromServices] IAuthEndpointHandler<TRequest> handler,
                        TRequest request,
                        ClaimsPrincipal user
                    ) => handler.Handle(request, user)
                )
                .AddValidator<TRequest>()
                .RequireAuthorization(policy);
        }
    }
}
