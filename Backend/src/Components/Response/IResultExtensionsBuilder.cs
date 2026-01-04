using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Response;

public static class IResultExtensionsBuilder
{
    public static IResult BadRequest(
        this IResultExtensions _,
        string title,
        string? detail = null
    ) =>
        new Result(StatusCodes.Status400BadRequest, title)
        {
            Detail = detail,
            ["type"] = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        };

    public static IResult Conflict(this IResultExtensions _, string? detail = null) =>
        new Result(StatusCodes.Status409Conflict, "A resource conflict occurs.")
        {
            Detail = detail,
            ["type"] = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        };

    public static Results<Ok<T>, NotFound> Data<T>(this IResultExtensions _, T? obj)
        where T : class => obj is null ? TypedResults.NotFound() : TypedResults.Ok(obj);

    public static Results<Ok<T>, NotFound> Data<T>(this IResultExtensions _, T? obj)
        where T : struct => obj is null ? TypedResults.NotFound() : TypedResults.Ok(obj.Value);

    public static Created<T> Created<T>(this IResultExtensions _, T data)
        where T : notnull => TypedResults.Created(string.Empty, data);
}
