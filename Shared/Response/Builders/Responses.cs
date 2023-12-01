using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Response.ReponseObjects;

namespace Shared.Response.Builders
{
    public static class Responses
    {
        public static Ok<DataResponse<T>> Data<T>(T obj)
            where T : notnull => TypedResults.Ok(new DataResponse<T>(obj));

        public static Ok<DataResponse> Data([DisallowNull] object obj) =>
            TypedResults.Ok(new DataResponse(obj));

        public static NoContent NoContent => TypedResults.NoContent();

        public static BadRequest<BadRequestResponse> BadRequest(
            string title,
            string detail = "",
            IDictionary<string, string[]>? errors = null
        ) => TypedResults.BadRequest(new BadRequestResponse(title, detail, errors));

        public static BadRequest<BadRequestResponse> ValidationFailure(
            IDictionary<string, string[]> objs
        ) => BadRequest("Validation failed.", "One or more validation errors occurred.", objs);

        public static BadRequest<BadRequestResponse> ValidationFailure(
            string parameter,
            string message
        ) => ValidationFailure(new Dictionary<string, string[]> { { parameter, [message] } });

        public static IResult TooManyRequests =>
            TypedResults.Problem(
                title: "Too many requests.",
                statusCode: StatusCodes.Status429TooManyRequests
            );
    }
}
