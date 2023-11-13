using Microsoft.AspNetCore.Mvc;

namespace Response.Builders
{
    public class ResponseErrorBuilder
    {
        internal ResponseErrorBuilder(int status, string message)
        {
            detail = message;
            this.status = status;
        }

        public ResponseErrorBuilder Add(string field, string message)
        {
            errors.Add(new ErrorObject(field, message));
            return this;
        }

        public ObjectResult Build()
        {
            return new ObjectResult(new ErrorResponseDto(status, detail, errors))
            {
                StatusCode = status,
                ContentTypes = new() { "application/json" }
            };
        }

        private readonly int status;
        private readonly string detail;
        private readonly ICollection<ErrorObject> errors = new List<ErrorObject>();
    }
}
