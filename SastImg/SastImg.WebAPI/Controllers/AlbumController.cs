using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SastImg.Application.AlbumServices.CreateAlbum;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.GetRemovedAlbums;
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
    /// Provides a set of APIs to manage albums.
    /// </summary>
    [ApiController]
    [Route("api/sastimg")]
    [Produces("application/json")]
    public sealed class AlbumController(
        IQueryRequestSender querySender,
        ICommandRequestSender commandSender
    ) : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// Get Detailed Album
        /// </summary>
        /// <remarks>
        /// Get detailed information of the specific album.
        /// </remarks>
        /// <param name="albumId">The album id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The album's detailed info</response>
        /// <response code="404">No album is found</response>
        [HttpGet("album/{albumId}")]
        [ProducesResponseType<DetailedAlbumDto>(StatusCodes.Status200OK)]
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
        /// Get Albums By UserId
        /// </summary>
        /// <remarks>
        /// Get albums that authored by the specific user.
        /// </remarks>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The albums</response>
        [Authorize]
        [HttpGet("user/{userId}/albums")]
        [ProducesResponseType<IEnumerable<UserAlbumDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<UserAlbumDto>>> GetUserAlbums(
            [Range(0, long.MaxValue)] long userId,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _querySender.QueryAsync(
                new GetUserAlbumsQuery(userId, User),
                cancellationToken
            );
            return Responses.Data(albums);
        }

        /// <summary>
        /// Search Albums
        /// </summary>
        /// <remarks>
        /// Search albums by category and title.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="categoryId">The category albums belong to.</param>
        /// <param name="page">24 albums per page.</param>
        /// <param name="title">The album title(Support approximate search)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The albums(order by update time descending)</response>
        [Authorize]
        [HttpGet("albums/search")]
        [ProducesResponseType<IEnumerable<SearchAlbumDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<SearchAlbumDto>>> SearchAlbums(
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
        /// Get Removed Albums
        /// </summary>
        /// <remarks>
        /// Get the removed albums of the specific user.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="userId">USER that removed albums authored by</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The removed albums</response>
        [Authorize]
        [HttpGet("user/{userId}/albums/removed")]
        [ProducesResponseType<IEnumerable<RemovedAlbumDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<RemovedAlbumDto>>> GetRemovedAlbums(
            [Range(0, long.MaxValue)] long userId,
            CancellationToken cancellationToken = default
        )
        {
            var albums = await _querySender.QueryAsync(
                new GetRemovedAlbumsQuery(userId, User),
                cancellationToken
            );
            return Responses.Data(albums);
        }

        /// <summary>
        /// Create Album
        /// </summary>
        /// <remarks>
        /// Create a new album.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="request">New album info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="201">The created album</response>
        [Authorize]
        [HttpPost("album")]
        [ProducesResponseType<CreateAlbumDto>(StatusCodes.Status201Created)]
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
        /// Update Album Info
        /// </summary>
        /// <remarks>
        /// Update the album's title, description, category, and accessibility.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="albumId">The album to be updated</param>
        /// <param name="request">The new album info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">The album is updated successfully.</response>
        [Authorize]
        [HttpPut("album/{albumId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// Remove Album
        /// </summary>
        /// <remarks>
        /// Remove the album.
        /// Data can be restored in a period of time
        /// until server deletes them permanently.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="albumId">The album to be removed.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">The album is removed successfully.</response>
        [Authorize]
        [HttpPut("album/{albumId}/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// Restore Album
        /// </summary>
        /// <remarks>
        /// Restore a removed album.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="albumId">The album to be restored</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">The album is restored successfully.</response>
        [Authorize]
        [HttpPut("album/{albumId}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// Update Album Collaborators
        /// </summary>
        /// <remarks>
        /// Update the album's collaborators.
        /// <para>Authorization is required.</para>
        /// </remarks>
        /// <param name="albumId">The album id.</param>
        /// <param name="request">List of updated collaborators.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">The album's collaborators are updated successfully.</response>
        [Authorize]
        [HttpPut("album/{albumId}/collaborators")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
