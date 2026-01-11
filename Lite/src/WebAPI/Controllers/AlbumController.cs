using System.ComponentModel.DataAnnotations;
using Application.AlbumServices.Queries;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Commands;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Infrastructure.Shared;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers;

[Route("api/albums")]
[ApiController]
[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
public sealed class AlbumController(IMediator mediator) : ControllerBase
{
    #region [Command/Post]

    public readonly record struct CreateAlbumRequest(
        AlbumTitle Title,
        AlbumDescription Description,
        CategoryId CategoryId,
        AccessLevel AccessLevel
    );

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] [Required] CreateAlbumRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateAlbumCommand command = new(
            request.Title,
            request.Description,
            request.AccessLevel,
            request.CategoryId,
            User
        );
        var id = await mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [Authorize]
    [HttpPost("{id:long}/remove")]
    public async Task<IActionResult> Remove(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        RemoveAlbumCommand command = new(id, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id:long}/restore")]
    public async Task<IActionResult> Restore(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        RestoreAlbumCommand command = new(id, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateAccessLevelRequest(AccessLevel AccessLevel);

    [Authorize]
    [HttpPost("{id:long}/accessLevel")]
    public async Task<IActionResult> UpdateAccessLevel(
        [FromRoute] AlbumId id,
        [FromBody] [Required] UpdateAccessLevelRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateAccessLevelCommand command = new(id, request.AccessLevel, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateDescriptionRequest(AlbumDescription Description);

    [Authorize]
    [HttpPost("{id:long}/description")]
    public async Task<IActionResult> UpdateDescription(
        [FromRoute] AlbumId id,
        [FromBody] [Required] UpdateDescriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateAlbumDescriptionCommand command = new(id, request.Description, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateTitleRequest(AlbumTitle Title);

    [Authorize]
    [HttpPost("{id:long}/title")]
    public async Task<IActionResult> UpdateTitle(
        [FromRoute] AlbumId id,
        [FromBody] [Required] UpdateTitleRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateAlbumTitleCommand command = new(id, request.Title, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateAlbumTagsRequest(AlbumTags Tags);

    [Authorize]
    [HttpPost("{id:long}/tags")]
    public async Task<IActionResult> UpdateTags(
        [FromRoute] AlbumId id,
        [FromBody] [Required] UpdateAlbumTagsRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateAlbumTagsCommand command = new(id, request.Tags, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateCollaboratorsRequest(Collaborators Collaborators);

    [Authorize]
    [HttpPost("{id:long}/collaborators")]
    public async Task<IActionResult> UpdateCollaborators(
        [FromRoute] AlbumId id,
        [FromBody] [Required] UpdateCollaboratorsRequest request
    )
    {
        UpdateCollaboratorsCommand command = new(id, request.Collaborators, User);
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id:long}/cover")]
    [RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024 * 20)]
    public async Task<IActionResult> UpdateCover(
        [FromRoute] AlbumId id,
        [FromForm] [FileValidator(0, 5)] IFormFile? file = null,
        CancellationToken cancellationToken = default
    )
    {
        IImageFile? cover = null;
        if (file is not null)
        {
            var image = ImageFile.Create(file.OpenReadStream());
            Response.RegisterForDispose(image);
            cover = image;
        }

        UpdateCoverCommand command = new(id, cover, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id:long}/subscribe")]
    public async Task<IActionResult> Subscribe(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        SubscribeCommand command = new(id, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id:long}/unsubscribe")]
    public async Task<IActionResult> Unsubscribe(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        UnsubscribeCommand command = new(id, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    #endregion

    #region [Query/Get]

    [HttpGet]
    [ResponseCache(
        Duration = 10,
        Location = ResponseCacheLocation.Any,
        VaryByQueryKeys = ["category", "author", "title"]
    )]
    public async Task<IActionResult> GetAlbums(
        [FromQuery] long? category = null,
        [FromQuery] long? author = null,
        [FromQuery] [MaxLength(AlbumTitle.MaxLength)] string? title = null,
        [FromQuery] long? cursor = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(
            new AlbumsQuery(category, author, title, cursor, User),
            cancellationToken
        );
        return this.DataOrNotFound(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetDetailedAlbum(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(new DetailedAlbumQuery(id, User), cancellationToken);
        return this.DataOrNotFound(result);
    }

    [HttpGet("removed")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> GetRemovedAlbums()
    {
        var result = await mediator.Send(new RemovedAlbumsQuery(User));
        return this.DataOrNotFound(result);
    }

    [HttpGet("{id:long}/cover")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetCover(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(new AlbumCoverQuery(id, User), cancellationToken);
        return this.ImageOrNotFound(result);
    }

    #endregion
}
