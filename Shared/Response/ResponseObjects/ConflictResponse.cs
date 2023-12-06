using Microsoft.AspNetCore.Http;

namespace Response.ResponseObjects
{
    public sealed class ConflictResponse(IDictionary<string, string> conflicts)
    {
        public string Type { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
        public string Title { get; } = "A resource conflict occurs.";
        public int Status { get; } = StatusCodes.Status409Conflict;
        public IDictionary<string, string> Conflicts { get; } = conflicts;
    }
}
