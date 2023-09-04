using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Response.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            this.errors ??= new List<ResponseErrorObject>();
            errors.Add(new ResponseErrorObject(field, message));
            return this;
        }

        public ResponseErrorBuilder Add(IEnumerable<ValidationFailure> errors)
        {
            this.errors ??= new List<ResponseErrorObject>();
            foreach (var error in errors)
            {
                this.errors.Add(new ResponseErrorObject(error.PropertyName, error.ErrorMessage));
            }
            return this;
        }

        public ResponseErrorBuilder Add(IEnumerable<IdentityError> errors)
        {
            this.errors ??= new List<ResponseErrorObject>();
            foreach (var error in errors)
            {
                this.errors.Add(new ResponseErrorObject(error.Code, error.Description));
            }
            return this;
        }

        public ObjectResult Build()
        {
            if (errors is null)
                return new GeneralError(status, detail);
            else
                return new DetailedError(status, detail, errors);
        }

        private readonly int status;
        private readonly string detail;
        private ICollection<ResponseErrorObject>? errors;
    }
}
