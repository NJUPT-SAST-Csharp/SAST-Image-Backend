using System.ComponentModel.DataAnnotations;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Application.ImageServices.GetAlbumImages;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.GetUserImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity.Commands;
using SastImg.Domain.AlbumTagEntity;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers;

/// <summary>
/// Controller for image related operations.
/// </summary>
[ApiController]
[Route("api/sastimg")]
[Produces("application/json")]
public sealed class ImageController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get Images by AlbumId
    /// </summary>
    /// <remarks>
    /// Get images by album id
    /// </remarks>
    /// <param name="id">Id of album that images belong to</param>
    /// <param name="page">24 images per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The images</response>
    [HttpGet("album/{id}/images")]
    [ProducesResponseType<IEnumerable<AlbumImageDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<AlbumImageDto>>> GetAlbumImages(
        [FromRoute] AlbumId id,
        [Range(0, 1000)] int page = 0,
        CancellationToken cancellationToken = default
    )
    {
        var images = await mediator.Send(new GetAlbumImages(id, page, User), cancellationToken);

        return Responses.Data(images);
    }

    /// <summary>
    /// Get User Images
    /// </summary>
    /// <remarks>
    /// Get images of a specific user directly.
    /// </remarks>
    /// <param name="id">The user id</param>
    /// <param name="page">24 images per page</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    [HttpGet("user/{id}/images")]
    [ProducesResponseType<IEnumerable<UserImageDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<UserImageDto>>> GetUserImages(
        [FromRoute] UserId id,
        [Range(0, 1000)] int page,
        CancellationToken cancellationToken
    )
    {
        var images = await mediator.Send(new GetUserImagesQuery(id, page, User), cancellationToken);
        return Responses.Data(images);
    }

    /// <summary>
    /// Search Images
    /// </summary>
    /// <remarks>
    /// Search images by a series of params.
    /// <para>Authorization is required</para>
    /// </remarks>
    /// <param name="tags">Tags that images have</param>
    /// <param name="categoryId">Category that (images') albums belong to</param>
    /// <param name="page">24 images per page</param>
    /// <param name="order">
    /// <para>Order of search results</para>
    /// <para>
    /// 0: order by date<br/>
    /// 1: order by likes<br/>
    /// 2: order by views
    /// </para>
    /// </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The images</response>
    [Authorize]
    [HttpGet("images")]
    [ProducesResponseType<IEnumerable<SearchedImageDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<SearchedImageDto>>> SearchImages(
        [FromQuery] [MaxLength(5)] long[] tags,
        [Range(0, int.MaxValue)] int categoryId = 0,
        [Range(0, 1000)] int page = 0,
        SearchOrder order = SearchOrder.ByDate,
        CancellationToken cancellationToken = default
    )
    {
        var images = await mediator.Send(
            new SearchImagesQuery(page, order, categoryId, tags, User),
            cancellationToken
        );
        return Responses.Data(images);
    }

    /// <summary>
    /// Get Detailed Image
    /// </summary>
    /// <remarks>
    /// Get detailed information of the specific image.
    /// </remarks>
    /// <param name="imageId">The image id</param>
    /// <param name="albumId">Album that the image belongs to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The image</response>
    /// <response code="404">No image found</response>
    [HttpGet("album/{albumId}/image/{imageId}")]
    [ProducesResponseType<DetailedImageDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<DetailedImageDto>, NotFound>> GetImage(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId,
        CancellationToken cancellationToken
    )
    {
        var image = await mediator.Send(
            new GetImageQuery(albumId, imageId, User),
            cancellationToken
        );
        return Responses.DataOrNotFound(image);
    }

    /// <summary>
    /// Get Removed Images
    /// </summary>
    /// <remarks>
    /// Get removed images in a specific album.
    /// The album must not be removed.
    /// <para>Authorization is required</para>
    /// </remarks>
    /// <param name="id">Album that removed images belong to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The removed images</response>
    [Authorize]
    [HttpGet("album/{id}/images/removed")]
    [ProducesResponseType<IEnumerable<AlbumImageDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<AlbumImageDto>>> GetRemovedImages(
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken
    )
    {
        var images = await mediator.Send(new GetRemovedImagesQuery(id, User), cancellationToken);
        return Responses.Data(images);
    }

    /// <summary>
    /// Remove Image
    /// </summary>
    /// <remarks>
    /// Remove an image from an album.
    /// Data can be restored in a period of time
    /// until server deletes them permanently.
    /// <para>Authorization is required</para>
    /// </remarks>
    /// <param name="albumId">Album that the image to be removed belongs to</param>
    /// <param name="imageId">The image id</param>
    /// <response code="204">The image is removed successfully</response>
    [Authorize]
    [HttpPut("album/{albumId}/image/{imageId}/remove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContent> RemoveImage(
        [FromRoute] AlbumId albumId,
        [FromRoute] ImageId imageId
    )
    {
        await mediator.Send(new RemoveImageCommand(albumId, imageId, User));
        return Responses.NoContent;
    }

    public readonly record struct AddImageRequest(
        ImageTitle Title,
        ImageDescription Description,
        ImageTagId[] Tags
    );

    /// <summary>
    /// Add Image
    /// </summary>
    /// <remarks>
    /// Add a new image to an album
    /// <para>Authorization is required</para>
    /// </remarks>
    /// <param name="request">The new image info.</param>
    /// <param name="id">Album that the image to be added belongs to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="201">The image is added successfully</response>
    [Authorize]
    [HttpPost("album/{id}/add")]
    [ProducesResponseType<ImageInfoDto>(StatusCodes.Status201Created)]
    public async Task<Created<ImageInfoDto>> AddImage(
        [FromForm] AddImageRequest request,
        [FromRoute] AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        var command = new AddImageCommand(
            request.Title,
            request.Description,
            request.Tags,
            id,
            User
        );

        var response = await mediator.Send(command, cancellationToken);
        return Responses.Created(response);
    }
}
