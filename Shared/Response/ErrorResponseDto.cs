namespace Shared.Response
{
    public class ErrorResponseDto
    {
        public ErrorResponseDto(int statusCode, string detail, ICollection<ErrorObject> errors)
        {
            Status = statusCode;
            Detail = detail;
            Errors = errors;
        }

        public int Status { get; init; }
        public string Detail { get; init; }
        public ICollection<ErrorObject> Errors { get; init; }
    }
}
