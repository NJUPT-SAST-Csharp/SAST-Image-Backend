using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SastImg.Application.AlbumServices.CreateAlbum;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.RemoveAlbum;
using SastImg.Application.AlbumServices.RestoreAlbum;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.Application.AlbumServices.UpdateAlbumInfo;
using SastImg.Application.AlbumServices.UpdateCollaborators;
using SastImg.WebAPI.Requests.AlbumRequest;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    [Route("api")]
    [ApiController]
    public sealed class AlbumController(
        IQueryRequestSender querySender,
        ICommandRequestSender commandSender
    ) : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet("albums")]
        public async Task<Ok<IEnumerable<AlbumDto>>> GetAlbums(
            [Range(0, long.MaxValue)] long userId,
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _querySender.QueryAsync(
                new GetAlbumsQuery(page, userId, User),
                cancellationToken
            );
            return Responses.Data(albums);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("album/{albumId}")]
        public async Task<Results<Ok<DetailedAlbumDto>, NotFound>> GetAlbum(
            [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _querySender.QueryAsync(
                new GetAlbumQuery(albumId, User),
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
        [HttpGet("albums/search")]
        public async Task<Ok<IEnumerable<AlbumDto>>> SearchAlbums(
            [Range(0, long.MaxValue)] long categoryId,
            [MaxLength(12)] string title,
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _querySender.QueryAsync(
                new SearchAlbumsQuery(categoryId, title, page, User),
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
        [HttpPost("album")]
        public async Task<Created<CreateAlbumDto>> CreateAlbum(
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
            var album = await _commandSender.CommandAsync(command, cancellationToken);
            return Responses.Created(album);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("album/{albumId}")]
        public async Task<NoContent> UpdateAlbumInfo(
            [FromRoute] [Range(0, long.MaxValue)] long albumId,
            [FromBody] UpdateAlbumInfoRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var command = new UpdateAlbumInfoCommand(
                albumId,
                request.Title,
                request.Description,
                request.CategoryId,
                request.Accessibility,
                User
            );
            await _commandSender.CommandAsync(command, cancellationToken);
            return Responses.NoContent;
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("album/{albumId}/remove")]
        public async Task<NoContent> RemoveAlbum(
            [FromRoute] [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new RemoveAlbumCommand(albumId, User),
                cancellationToken
            );
            return Responses.NoContent;
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("album/{albumId}/restore")]
        public async Task<NoContent> RestoreAlbum(
            [FromRoute] [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new RestoreAlbumCommand(albumId, User),
                cancellationToken
            );
            return Responses.NoContent;
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("album/{albumId}/collaborators")]
        public async Task<NoContent> UpdateCollaborators(
            [FromRoute] [Range(0, long.MaxValue)] long albumId,
            [FromBody] UpdateCollaboratorsRequest request,
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new UpdateCollaboratorsCommand(albumId, request.Collaborators, User),
                cancellationToken
            );
            return Responses.NoContent;
        }
    }
}
