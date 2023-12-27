using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
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
        /// <param name="categoryId"></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet]
        public async Task<Ok<IEnumerable<AlbumDto>>> GetAlbums(
            [Range(0, 10000)] int page = 0,
            [Range(0, long.MaxValue)] long userId = 0,
            [Range(0, long.MaxValue)] long categoryId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _request.Send(
                new GetAlbumsQueryRequest(page, userId, categoryId, new(User)),
                cancellationToken
            );
            return Responses.Data(albums);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{albumId}")]
        public async Task<Results<Ok<DetailedAlbumDto>, NotFound>> GetAlbum(
            [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _request.Send(
                new GetAlbumQueryRequest(albumId, User),
                cancellationToken
            );
            return Responses.DataOrNotFound(album);
        }
    }
}
