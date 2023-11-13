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

        public static ObjectResult Data(object data) =>
            new(data)
            {
                StatusCode = StatusCodes.Status200OK,
                ContentTypes = new() { "application/json" }
            };
    }
}
