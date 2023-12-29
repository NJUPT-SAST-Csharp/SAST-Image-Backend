using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    /// <param name="sender"></param>
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AlbumController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet]
        public async Task<Ok<IEnumerable<AlbumDto>>> GetAlbums(
            CancellationToken cancellationToken,
            [Range(0, 1000)] int page = 0,
            [Range(0, long.MaxValue)] long userId = 0
        )
        {
            var albums = await _sender.Send(
                new GetAlbumsQueryRequest(page, userId, User),
                cancellationToken
            );
            return Responses.Data(albums);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{albumId}")]
        public async Task<Results<Ok<DetailedAlbumDto>, NotFound>> GetAlbum(
            CancellationToken cancellationToken,
            [Range(0, long.MaxValue)] long albumId
        )
        {
            var album = await _sender.Send(
                new GetAlbumQueryRequest(albumId, User),
                cancellationToken
            );
            return Responses.DataOrNotFound(album);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="categoryId"></param>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("search")]
        public async Task<Ok<IEnumerable<AlbumDto>>> SearchAlbums(
            CancellationToken cancellationToken,
            [Range(0, long.MaxValue)] long categoryId,
            [Range(0, 1000)] int page = 0,
            [MaxLength(10)] string title = ""
        )
        {
            // TODO: implement
            return Responses.Data<IEnumerable<AlbumDto>>([]);
        }
    }
}
