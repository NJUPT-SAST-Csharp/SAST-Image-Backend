using Microsoft.AspNetCore.Http;
using Response.ResponseObjects;

namespace Response.ReponseObjects
{
    public sealed class BadRequestResponse(
        string title,
        string? detail = null,
        IDictionary<string, string[]>? errors = null
    ) : ResponseObject
    {
        public override string Type { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

        public override string Title { get; } = title;

        public override int Status { get; } = StatusCodes.Status400BadRequest;

        public string? Detail { get; } = detail;

        public IDictionary<string, string[]>? Errors { get; } = errors;
    }
}
