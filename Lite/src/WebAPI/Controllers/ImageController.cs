using System.ComponentModel.DataAnnotations;
using Application.ImageServices;
using Application.ImageServices.Queries;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.ImageEntity;
using Infrastructure.Shared;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers;

[Route("api")]
[ApiController]
public class ImageController(IMediator mediator) : ControllerBase
{
    #region [Command/Post]

    [Authorize]
    [RequestSizeLimit(1024 * 1024 * 50)]
    [HttpPost("albums/{albumId:long}/add")]
    public async Task<IActionResult> AddImage(
        [FromRoute] AlbumId albumId,
        [FromForm] [Required] [FileValidator(0, 50)] IFormFile image,
        [FromForm] [MaxLength(ImageTitle.MaxLength)] string title,
        [FromForm] [Length(0, 10)] string[]? tags = null,
        CancellationToken cancellationToken = default
    )
    {
        using var file = ImageFile.Create(image.OpenReadStream());

        AddImageCommand command = new(
            albumId,
            title.Bind<ImageTitle>(),
            tags is null ? default : tags.Bind<ImageTags, string[]>(),
            file,
            User
        );
        var id = await mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    public readonly record struct UpdateImageTagsRequest(ImageTags Tags);

    [Authorize]
    [HttpPost("albums/{albumId:long}/images/{imageId:long}/tags")]
    public async Task<IActionResult> UpdateTags(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        [FromBody] UpdateImageTagsRequest request
    )
    {
        UpdateImageTagsCommand command = new(albumId, imageId, request.Tags, User);
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpPost("albums/{albumId:long}/images/{imageId:long}/remove")]
    public async Task<IActionResult> Remove(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        CancellationToken cancellationToken
    )
    {
        RemoveImageCommand command = new(albumId, imageId, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("albums/{albumId:long}/images/{imageId:long}/restore")]
    public async Task<IActionResult> Restore(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        CancellationToken cancellationToken
    )
    {
        RestoreImageCommand command = new(albumId, imageId, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("albums/{albumId:long}/images/{imageId:long}/like")]
    public async Task<IActionResult> Like(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        CancellationToken cancellationToken
    )
    {
        LikeImageCommand command = new(albumId, imageId, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("albums/{albumId:long}/images/{imageId:long}/unlike")]
    public async Task<IActionResult> Unlike(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        CancellationToken cancellationToken
    )
    {
        UnlikeImageCommand command = new(albumId, imageId, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("albums/{albumId:long}/images/{imageId:long}")]
    public async Task<IActionResult> Delete(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        CancellationToken cancellationToken
    )
    {
        DeleteImageCommand command = new(albumId, imageId, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    #endregion

    #region [Query/Get]


    [HttpGet("images")]
    [ResponseCache(
        Duration = 10,
        Location = ResponseCacheLocation.Any,
        VaryByQueryKeys = ["uploader", "album", "page"]
    )]
    public async Task<IActionResult> GetImages(
        [FromQuery] long? uploader = null,
        [FromQuery] long? album = null,
        [FromQuery] int page = 0,
        CancellationToken cancellationToken = default
    )
    {
        ImagesQuery query = new(uploader, album, page, User);
        var images = await mediator.Send(query, cancellationToken);
        return this.DataOrNotFound(images);
    }

    [HttpGet("images/{id:long}")]
    public async Task<IActionResult> GetImage(
        [FromRoute] ImageId id,
        [FromQuery] ImageKind kind = ImageKind.Thumbnail,
        CancellationToken cancellationToken = default
    )
    {
        ImageFileQuery query = new(id, kind, User);
        var image = await mediator.Send(query, cancellationToken);
        return this.ImageOrNotFound(image);
    }

    [Authorize]
    [HttpGet("images/{id:long}/info")]
    public async Task<IActionResult> GetDetailedImage(
        [FromRoute] ImageId id,
        CancellationToken cancellationToken
    )
    {
        DetailedImageQuery query = new(id, User);
        var image = await mediator.Send(query, cancellationToken);
        return this.DataOrNotFound(image);
    }

    [Authorize]
    [HttpGet("albums/{albumId:long}/images/removed")]
    public async Task<IActionResult> GetRemovedImages(
        [FromRoute] AlbumId albumId,
        CancellationToken cancellationToken
    )
    {
        RemovedImagesQuery query = new(albumId, User);
        var images = await mediator.Send(query, cancellationToken);
        return this.DataOrNotFound(images);
    }

    #endregion
}
