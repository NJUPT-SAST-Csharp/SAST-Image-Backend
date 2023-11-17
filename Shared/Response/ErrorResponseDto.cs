namespace Shared.Response
{
    public sealed class ErrorResponseDto(
        int statusCode,
        string detail,
        ICollection<ErrorObject> errors
    )
    {
        public int Status { get; init; } = statusCode;
        public string Detail { get; init; } = detail;
        public ICollection<ErrorObject> Errors { get; init; } = errors;
    }
}
