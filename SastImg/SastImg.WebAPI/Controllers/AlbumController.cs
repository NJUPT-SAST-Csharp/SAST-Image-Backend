using MediatR;
using Microsoft.AspNetCore.Mvc;
using Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly ISender _request;

        public AlbumController(ISender request)
        {
            _request = request;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums(CancellationToken cancellationToken)
        {
            return ResponseBuilder.Data(new List<int>());
        }
    }
}
