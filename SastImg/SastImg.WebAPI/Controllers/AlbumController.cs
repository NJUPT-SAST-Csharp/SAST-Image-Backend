using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Primitives.Common.Policies;
using SastImg.Application.Albums.GetAlbums;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    /// <param name="request"></param>
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AlbumController(ISender request) : ControllerBase
    {
        private readonly ISender _request = request;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(
            CacheProfileName = RateLimiterPolicyNames.Default,
            VaryByQueryKeys = ["page", "userId"]
        )]
        public async Task<IResult> GetAlbums(
            [Range(0, 10000)] int page = 0,
            [Range(0, long.MaxValue)] long userId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _request.Send(
                new GetAlbumsQuery(User, page, userId),
                cancellationToken
            );
            return ResponseBuilder.Data(nameof(albums), albums);
        }
    }
}
