using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Primitives.Common.Policies;
using SastImg.Application.Albums.GetAlbums;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AlbumController : ControllerBase
    {
        private readonly ISender _request;

        public AlbumController(ISender request)
        {
            _request = request;
        }

        [HttpGet]
        [ResponseCache(
            CacheProfileName = RateLimiterPolicyNames.Default,
            VaryByQueryKeys = [ "page" ]
        )]
        public async Task<IActionResult> GetAlbums(
            [Range(0, 10000000)] int page = 0,
            [Range(0, long.MaxValue)] long userId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _request.Send(new GetAlbumsQuery(page, userId), cancellationToken);
            return ResponseBuilder.Data(albums);
        }

        //[HttpGet]
        //[ResponseCache(CacheProfileName = RateLimiterPolicyNames.Default, VaryByQueryKeys = [ ])]
        //public async Task<IActionResult> GetAlbumsByUserId(
        //    long userId,
        //    CancellationToken cancellationToken
        //) { }
    }
}
