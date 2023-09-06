using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Response
{
    public class ErrorResponseDto
    {
        public ErrorResponseDto(int statusCode, string detail, ICollection<ErrorObject> errors)
        {
            this.Status = statusCode;
            this.Detail = detail;
            this.Errors = errors;
        }

        public int Status { get; init; }
        public string Detail { get; init; }
        public ICollection<ErrorObject> Errors { get; init; }
    }
}
