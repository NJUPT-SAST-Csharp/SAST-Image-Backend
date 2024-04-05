using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace FoxResult.Extensions
{
    public static class ResultsExtensions
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions =
            new(JsonSerializerDefaults.Web);

        static ResultsExtensions()
        {
            _jsonSerializerOptions.Converters.Add(new JsonLongConverter());
        }

        public static IResult From(this IResultExtensions _, Result result)
        {
            if (result.IsFailure)
            {
                var title = Enum.GetName((HttpStatusCode)result.Error.Code);

                return Results.Problem(
                    new()
                    {
                        Status = result.Error.Code,
                        Detail = result.Error.Message,
                        Title = title
                    }
                );
            }
            else
            {
                return Results.NoContent();
            }
        }

        public static IResult From<T>(this IResultExtensions _, Result<T> result)
        {
            if (result.IsFailure)
            {
                var title = Enum.GetName((HttpStatusCode)result.Error.Code);

                return Results.Problem(
                    new()
                    {
                        Status = result.Error.Code,
                        Detail = result.Error.Message,
                        Title = title
                    }
                );
            }
            else
            {
                return Results.Json(result.Value, _jsonSerializerOptions);
            }
        }

        public static Task<IResult> FromTask(this IResultExtensions _, Task<Result> task)
        {
            return task.ContinueWith(t => From(_, t.Result));
        }

        public static Task<IResult> FromTask<T>(this IResultExtensions _, Task<Result<T>> task)
        {
            return task.ContinueWith(t => From(_, t.Result));
        }

        public static IResult Custom<T>(this IResultExtensions _, T value, int code)
        {
            return Results.Json(value, _jsonSerializerOptions, "application/json", code);
        }

        public static IResult Custom(this IResultExtensions _, object value, int code)
        {
            return Results.Json(value, _jsonSerializerOptions, "application/json", code);
        }

        public static IResult Custom(
            this IResultExtensions _,
            Dictionary<string, object> args,
            int code
        )
        {
            return Results.Json(args, _jsonSerializerOptions, "application/json", code);
        }
    }
}
