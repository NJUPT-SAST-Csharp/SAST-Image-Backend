using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Response.Builders
{
    public static class ResponseBuilder
    {
        public static ResponseErrorBuilder Error(int status, string detail) => new(status, detail);

        public static ResponseErrorBuilder BadRequest(
            string detail = ResponseMessages.InvalidParameters
        ) => new(StatusCodes.Status400BadRequest, detail);

        public static ObjectResult TooManyRequests =>
            new ResponseErrorBuilder(
                StatusCodes.Status429TooManyRequests,
                ResponseMessages.TooManyRequests
            ).Build();

        public static ObjectResult Data(object data) =>
            new(new { data, status = StatusCodes.Status200OK })
            {
                StatusCode = StatusCodes.Status200OK,
                ContentTypes =  [ "application/json" ]
            };
    }
}
