using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Response.ReponseObjects;
using Response.ResponseObjects;

namespace Shared.Response.Builders
{
    public static class Responses
    {
        public static Results<Ok<T>, NotFound> DataOrNotFound<T>(T? obj)
        {
            if (obj is not null)
                return TypedResults.Ok(obj);
            else
                return TypedResults.NotFound();
        }

        public static Ok<T> Data<T>(T obj) => TypedResults.Ok(obj);

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

        public static ProblemHttpResult TooManyRequests =>
            TypedResults.Problem(
                title: "Too many requests.",
                statusCode: StatusCodes.Status429TooManyRequests
            );

        public static Conflict<ConflictResponse> Conflict(string fieldName, string value) =>
            TypedResults.Conflict(
                new ConflictResponse(new Dictionary<string, string> { { fieldName, value } })
            );

        public static Conflict<ConflictResponse> Conflict(IDictionary<string, string> conflicts) =>
            TypedResults.Conflict(new ConflictResponse(conflicts));

        public static Created<T> Created<T>(T data)
            where T : notnull => TypedResults.Created(string.Empty, data);
    }
}
