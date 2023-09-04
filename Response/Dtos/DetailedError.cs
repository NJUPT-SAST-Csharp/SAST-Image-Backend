using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Response.Dtos
{
    public class DetailedError : ObjectResult
    {
        internal DetailedError(int status, string detail, ICollection<ResponseErrorObject> errors)
            : base(
                new
                {
                    status,
                    detail,
                    errors
                }
            )
        {
            StatusCode = status;
        }
    }
}
