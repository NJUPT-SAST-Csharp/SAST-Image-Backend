using MediatR;
using Microsoft.AspNetCore.Mvc;
using Primitives.Common.Policies;
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
        [ResponseCache(
            CacheProfileName = RateLimiterPolicies.Default,
            VaryByQueryKeys = [ "page" ]
        )]
        public async Task<IActionResult> GetAlbums(int page, CancellationToken cancellationToken)
        {
            var albums = await _request.Send(new GetAlbumsQuery(page), cancellationToken);
            return ResponseBuilder.Data(albums);
        }
    }
}
