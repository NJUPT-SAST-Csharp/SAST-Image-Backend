using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response.Builders;

namespace Response
{
    public static class ResponseDispatcher
    {
        public static ResponseErrorBuilder Error(int status, string detail) => new(status, detail);

        public static ObjectResult Data(object data) =>
            new(data)
            {
                StatusCode = StatusCodes.Status200OK,
                ContentTypes = new() { "application/json" }
            };
    }
}
