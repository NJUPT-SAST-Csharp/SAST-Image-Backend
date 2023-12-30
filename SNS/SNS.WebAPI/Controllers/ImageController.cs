using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Response.Builders;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ImageController : ControllerBase
    {
        [HttpGet]
        public async Task<Ok<long[]>> GetImagesRank(
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
        )
        {
            return Responses.Data(new long[] { 1, 2, 3 });
        }
    }
}
