using System.Security.Claims;
using Account.WebAPI.SeedWorks;
using Auth.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using Response.Extensions;
using Shared.Primitives.Query;
using Shared.Response.Builders;

namespace Account.WebAPI.Configurations
{
    internal static class EndpointExtentions
    {
        public static RouteHandlerBuilder AddAuthorization(
            this RouteHandlerBuilder builder,
            params AuthorizationRole[] roles
        )
        {
            builder.RequireAuthorization(Array.ConvertAll(roles, r => r.ToString()));

            builder.WithUnauthorizedResponse();

            return builder;
        }

        public static RouteHandlerBuilder AddValidator<TRequest>(this RouteHandlerBuilder builder)
            where TRequest : IBaseRequestObject
        {
            builder.AddEndpointFilter<ValidationFilter<TRequest>>();
            return builder;
        }

        public static RouteHandlerBuilder AddPost<TRequest, TProcessRequest>(
            this RouteGroupBuilder builder,
            string route,
            Func<TRequest, ClaimsPrincipal, TProcessRequest> mapper
        )
            where TRequest : struct, ICommandRequestObject<TProcessRequest>
            where TProcessRequest : ICommandRequest
        {
            var handler = builder.MapPost(
                route,
                async (
                    [AsParameters] TRequest request,
                    [FromServices] ICommandRequestSender sender,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken
                ) =>
                {
                    await sender.CommandAsync(mapper(request, user), cancellationToken);
                    return Responses.NoContent;
                }
            );

            handler.WithNoContentResponse();

            return handler;
        }

        public static RouteHandlerBuilder AddPost<TRequest, TProcessRequest, TResponse>(
            this RouteGroupBuilder builder,
            string route,
            Func<TRequest, ClaimsPrincipal, TProcessRequest> mapper
        )
            where TRequest : struct, ICommandRequestObject<TProcessRequest, TResponse>
            where TProcessRequest : ICommandRequest<TResponse>
        {
            var handler = builder.MapPost(
                route,
                async (
                    [AsParameters] TRequest request,
                    [FromServices] ICommandRequestSender sender,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await sender.CommandAsync(
                        mapper(request, user),
                        cancellationToken
                    );
                    return Responses.DataOrNotFound(result);
                }
            );

            handler.WithDataResponse<TResponse>();

            return handler;
        }

        public static RouteHandlerBuilder AddPut<TRequest, TProcessRequest>(
            this RouteGroupBuilder builder,
            string route,
            Func<TRequest, ClaimsPrincipal, TProcessRequest> mapper
        )
            where TRequest : struct, ICommandRequestObject<TProcessRequest>
            where TProcessRequest : ICommandRequest
        {
            var handler = builder.MapPut(
                route,
                async (
                    [AsParameters] TRequest request,
                    [FromServices] ICommandRequestSender sender,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken
                ) =>
                {
                    await sender.CommandAsync(mapper(request, user), cancellationToken);
                    return Responses.NoContent;
                }
            );

            handler.WithNoContentResponse();

            return handler;
        }

        public static RouteHandlerBuilder AddGet<TRequest, TProcessRequest, TResponse>(
            this RouteGroupBuilder builder,
            string route,
            Func<TRequest, ClaimsPrincipal, TProcessRequest> mapper
        )
            where TRequest : struct, IQueryRequestObject<TProcessRequest, TResponse>
            where TProcessRequest : IQueryRequest<TResponse>
        {
            var handler = builder.MapGet(
                route,
                async (
                    [AsParameters] TRequest request,
                    [FromServices] IQueryRequestSender sender,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await sender.QueryAsync(mapper(request, user), cancellationToken);
                    Responses.DataOrNotFound(result);
                }
            );

            handler.WithDataResponse<TResponse>();

            return handler;
        }
    }
}
