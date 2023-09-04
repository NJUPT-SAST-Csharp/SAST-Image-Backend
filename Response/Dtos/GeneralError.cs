using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Response.Dtos
{
    public class GeneralError : ObjectResult
    {
        internal GeneralError(int status, string detail)
            : base(new { status, detail })
        {
            StatusCode = status;
        }
    }
}
