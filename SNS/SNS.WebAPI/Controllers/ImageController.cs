using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ImageController : ControllerBase
    {
        [NonAction]
        public async Task<Results<Ok, NoContent>> AddNewImageAsync()
        {
            return Results.Ok();
        }
    }
}
