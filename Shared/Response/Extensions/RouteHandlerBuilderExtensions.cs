using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Response.ReponseObjects;

namespace Response.Extensions
{
    public static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder WithBadRequestResponse(this RouteHandlerBuilder builder)
        {
            builder.Produces<BadRequest<BadRequestResponse>>(StatusCodes.Status400BadRequest);
            return builder;
        }

        public static RouteHandlerBuilder WithDataResponse<T>(this RouteHandlerBuilder builder)
        {
            builder.Produces<T>(StatusCodes.Status200OK);
            return builder;
        }

        public static RouteHandlerBuilder WithUnauthorizedResponse(this RouteHandlerBuilder builder)
        {
            builder.Produces<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized);
            return builder;
        }

        public static RouteHandlerBuilder WithNoContentResponse(this RouteHandlerBuilder builder)
        {
            builder.Produces<NoContent>(StatusCodes.Status204NoContent);
            return builder;
        }
    }
}
