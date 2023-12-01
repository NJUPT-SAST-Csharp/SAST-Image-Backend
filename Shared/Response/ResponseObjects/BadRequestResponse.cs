using Microsoft.AspNetCore.Http;

namespace Response.ReponseObjects
{
    public sealed class BadRequestResponse(
        string title,
        string detail,
        IDictionary<string, string[]>? errors = null
    )
    {
        public string Type { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

        public string Title { get; } = title;

        public int Status { get; } = StatusCodes.Status400BadRequest;

        public string Detail { get; } = detail;

        public IDictionary<string, string[]>? Errors { get; } = errors;
    }
}
