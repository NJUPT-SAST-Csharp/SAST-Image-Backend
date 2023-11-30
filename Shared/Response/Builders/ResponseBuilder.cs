using Microsoft.AspNetCore.Http;

namespace Shared.Response.Builders
{
    public static class ResponseBuilder
    {
        public static IResult Data(IDictionary<string, object> objs) =>
            TypedResults.Ok(new { data = objs });

        public static IResult Data(string key, object value) =>
            Data(new Dictionary<string, object> { { key, value } });

        public static IResult NoContent => TypedResults.NoContent();

        public static IResult BadRequest(string title, string? detail = null) =>
            TypedResults.BadRequest(
                new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title,
                    status = StatusCodes.Status400BadRequest,
                    detail
                }
            );

        public static IResult ValidationFailure(IDictionary<string, string[]> objs) =>
            TypedResults.ValidationProblem(
                type: "https://tools.ietf.org/html/rfc9110#section-15.3.1",
                title: "One or more validation errors occurred.",
                extensions: new Dictionary<string, object?>
                {
                    { "status", StatusCodes.Status400BadRequest }
                },
                errors: objs
            );

        public static IResult ValidationFailure(string parameter, string message) =>
            ValidationFailure(new Dictionary<string, string[]> { { parameter, [message] } });

        public static IResult TooManyRequests =>
            TypedResults.Problem(
                title: "Too many requests.",
                statusCode: StatusCodes.Status429TooManyRequests
            );
    }
}
