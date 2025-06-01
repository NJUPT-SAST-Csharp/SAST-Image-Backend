using System.ComponentModel.DataAnnotations;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.AlbumAggregate.CreateAlbum;
using SastImg.Application.AlbumAggregate.GetAlbums;
using SastImg.Application.AlbumAggregate.GetDetailedAlbum;
using SastImg.Application.AlbumAggregate.GetRemovedAlbums;
using SastImg.Application.AlbumAggregate.GetUserAlbums;
using SastImg.Application.AlbumAggregate.SearchAlbums;
using SastImg.Application.AlbumAggregate.UpdateAlbumInfo;
using SastImg.Application.AlbumAggregate.UpdateCollaborators;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Commands;
using SastImg.Domain.CategoryEntity;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers;

/// <summary>
/// Provides a set of APIs to manage albums.
/// </summary>
[ApiController]
[Route("api/sastimg")]
[Produces("application/json")]
public sealed class AlbumController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get Albums
    /// </summary>
    /// <remarks>
    /// Get albums by category.
    /// </remarks>
    /// <param name="id">The category id. When id=0, get all available albums.</param>
    /// <param name="page">The page id.</param>
    /// <param name="cancellationToken">Cancellatin token</param>
    /// <response code="200">The albums</response>
    [HttpGet("albums")]
    [ProducesResponseType<IEnumerable<AlbumDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<AlbumDto>>> GetAlbums(
        [FromQuery] CategoryId id,
        [Range(0, 1000)] int page = 0,
        CancellationToken cancellationToken = default
    )
    {
        var albums = await mediator.Send(new GetAlbumsQuery(id, page, User), cancellationToken);
        return Responses.Data(albums);
    }

    /// <summary>
    /// Get Detailed Album
    /// </summary>
    /// <remarks>
    /// Get detailed information of the specific album.
    /// </remarks>
    /// <param name="id">The album id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The album's detailed info</response>
    /// <response code="404">No album is found</response>
    [HttpGet("album/{id}")]
    [ProducesResponseType<DetailedAlbumDto>(StatusCodes.Status200OK)]
    public async Task<Results<Ok<DetailedAlbumDto>, NotFound>> GetDetailedAlbum(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        var album = await mediator.Send(new GetDetailedAlbumQuery(id, User), cancellationToken);
        return Responses.DataOrNotFound(album);
    }

    /// <summary>
    /// Get Albums By UserId
    /// </summary>
    /// <remarks>
    /// Get albums that authored by the specific user.
    /// </remarks>
    /// <param name="id">The user id.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The albums</response>
    [Authorize]
    [HttpGet("user/{id}/albums")]
    [ProducesResponseType<IEnumerable<UserAlbumDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<UserAlbumDto>>> GetUserAlbums(
        [FromRoute] UserId id,
        CancellationToken cancellationToken = default
    )
    {
        var albums = await mediator.Send(new GetUserAlbumsQuery(id, User), cancellationToken);
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
        [FromQuery] CategoryId categoryId,
        [FromQuery] string title,
        [Range(0, 1000)] int page = 0,
        CancellationToken cancellationToken = default
    )
    {
        var albums = await mediator.Send(
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
    /// <param name="id">USER that removed albums authored by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The removed albums</response>
    [Authorize]
    [HttpGet("user/{id}/albums/removed")]
    [ProducesResponseType<IEnumerable<RemovedAlbumDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<RemovedAlbumDto>>> GetRemovedAlbums(
        [FromRoute] UserId id,
        CancellationToken cancellationToken = default
    )
    {
        var albums = await mediator.Send(new GetRemovedAlbumsQuery(id, User), cancellationToken);
        return Responses.Data(albums);
    }

    public readonly record struct CreateAlbumRequest(
        AlbumTitle Title,
        AlbumDescription Description,
        CategoryId CategoryId,
        Accessibility Accessibility
    );

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
        var album = await mediator.Send(command, cancellationToken);
        return Responses.Created(album);
    }

    public readonly record struct UpdateAlbumInfoRequest(
        AlbumTitle Title,
        AlbumDescription Description,
        CategoryId CategoryId,
        Accessibility Accessibility
    );

    /// <summary>
    /// Update Album Info
    /// </summary>
    /// <remarks>
    /// Update the album's title, description, category, and accessibility.
    /// <para>Authorization is required.</para>
    /// </remarks>
    /// <param name="id">The album to be updated</param>
    /// <param name="request">The new album info</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">The album is updated successfully.</response>
    [Authorize]
    [HttpPut("album/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContent> UpdateAlbumInfo(
        [FromRoute] AlbumId id,
        [FromBody] UpdateAlbumInfoRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = new UpdateAlbumInfoCommand(
            id,
            request.Title,
            request.Description,
            request.CategoryId,
            request.Accessibility,
            User
        );

        await mediator.Send(command, cancellationToken);
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
    /// <param name="id">The album to be removed.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">The album is removed successfully.</response>
    [Authorize]
    [HttpPut("album/{id}/remove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContent> RemoveAlbum(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(new RemoveAlbumCommand(id, User), cancellationToken);
        return Responses.NoContent;
    }

    /// <summary>
    /// Restore Album
    /// </summary>
    /// <remarks>
    /// Restore a removed album.
    /// <para>Authorization is required.</para>
    /// </remarks>
    /// <param name="id">The album to be restored</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">The album is restored successfully.</response>
    [Authorize]
    [HttpPut("album/{id}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContent> RestoreAlbum(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(new RestoreAlbumCommand(id, User), cancellationToken);
        return Responses.NoContent;
    }

    public readonly record struct UpdateCollaboratorsRequest(UserId[] Collaborators);

    /// <summary>
    /// Update Album Collaborators
    /// </summary>
    /// <remarks>
    /// Update the album's collaborators.
    /// <para>Authorization is required.</para>
    /// </remarks>
    /// <param name="id">The album id.</param>
    /// <param name="request">List of updated collaborators.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">The album's collaborators are updated successfully.</response>
    [Authorize]
    [HttpPut("album/{id}/collaborators")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContent> UpdateCollaborators(
        [FromRoute] AlbumId id,
        [FromBody] UpdateCollaboratorsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(
            new UpdateCollaboratorsCommand(id, request.Collaborators, User),
            cancellationToken
        );

        return Responses.NoContent;
    }
}
