using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Response.ReponseObjects;

namespace Response.Extensions
{
    public static class RouteHandlerBuilderExtension
    {
        public static RouteHandlerBuilder WithValidationFailureResponse(
            this RouteHandlerBuilder builder
        )
        {
            builder.Produces<BadRequest<BadRequestResponse>>(StatusCodes.Status400BadRequest);
            return builder;
        }

        public static RouteHandlerBuilder WithBadRequestResponse(this RouteHandlerBuilder builder)
        {
            builder.Produces<BadRequest<BadRequestResponse>>(StatusCodes.Status400BadRequest);
            return builder;
        }

        public static RouteHandlerBuilder WithDataResponse<T>(this RouteHandlerBuilder builder)
            where T : notnull
        {
            builder.Produces<DataResponse<T>>(StatusCodes.Status200OK);
            return builder;
        }
    }
}
