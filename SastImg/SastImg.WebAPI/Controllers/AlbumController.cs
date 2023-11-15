using MediatR;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.Albums.GetAlbums;
using Shared.Response.Builders;

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
            var albums = await _request.Send(new GetAlbumsQuery(), cancellationToken);
            return ResponseBuilder.Data(albums);
        }
    }
}
