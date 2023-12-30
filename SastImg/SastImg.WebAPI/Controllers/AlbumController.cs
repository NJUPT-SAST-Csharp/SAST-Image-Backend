using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.AlbumServices.CreateAlbum;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.WebAPI.Requests.AlbumRequest;
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
            [Range(0, long.MaxValue)] long userId,
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
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
            [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
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
        //[EnableRateLimiting("")]
        public async Task<Ok<IEnumerable<AlbumDto>>> SearchAlbums(
            [Range(0, long.MaxValue)] long categoryId,
            [MaxLength(10)] string title,
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _sender.Send(
                new SearchAlbumsQueryRequest(categoryId, title, page, User),
                cancellationToken
            );
            return Responses.Data(albums);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<Ok<CreateAlbumDto>> CreateAlbum(
            [FromBody] CreateAlbumRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var command = new CreateAlbumCommand(
                request.Title,
                request.Description,
                request.CategoryId,
                request.Accessibility,
                User
            );
            var album = await _sender.Send(command, cancellationToken);
            return Responses.Data(album);
        }
    }
}
