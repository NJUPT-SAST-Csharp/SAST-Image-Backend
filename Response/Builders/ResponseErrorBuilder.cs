using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
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

        public ResponseErrorBuilder Add(IEnumerable<ValidationFailure> errors)
        {
            foreach (var error in errors)
            {
                this.errors.Add(new ErrorObject(error.PropertyName, error.ErrorMessage));
            }
            return this;
        }

        public ResponseErrorBuilder Add(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                this.errors.Add(new ErrorObject(error.Code, error.Description));
            }
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
