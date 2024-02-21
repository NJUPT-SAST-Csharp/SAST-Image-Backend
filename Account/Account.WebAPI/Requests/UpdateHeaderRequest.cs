using Account.Application.UserServices.UpdateHeader;
using Account.WebAPI.SeedWorks;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct UpdateHeaderRequest : ICommandRequestObject<UpdateHeaderCommand>
    {
        [FromForm]
        public readonly IFormFile HeaderFile { get; init; }
    }
}
