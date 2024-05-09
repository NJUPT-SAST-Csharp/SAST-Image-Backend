using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImageFile;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.GetUserImages;
using SastImg.Application.ImageServices.RemoveImage;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.WebAPI.Requests.ImageRequest;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// Controller for image related operations.
    /// </summary>
    [ApiController]
    [Route("api/sastimg")]
    [Produces("application/json")]
    public sealed class ImageController(
        IQueryRequestSender querySender,
        ICommandRequestSender commandSender
    ) : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// Get Images by AlbumId
        /// </summary>
        /// <remarks>
        /// Get images by album id
        /// </remarks>
        /// <param name="albumId">Id of album that images belong to</param>
        /// <param name="page">24 images per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The images</response>
        [HttpGet("album/{albumId}/images")]
        [ProducesResponseType<IEnumerable<AlbumImageDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<AlbumImageDto>>> GetImages(
            [Range(0, long.MaxValue)] long albumId = 0,
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
        )
        {
            var images = await _querySender.QueryAsync(
                new GetImagesQuery(albumId, page, User),
                cancellationToken
            );

            return Responses.Data(images);
        }

        /// <summary>
        /// Get User Images
        /// </summary>
        /// <remarks>
        /// Get images of a specific user directly.
        /// </remarks>
        /// <param name="userId">The user id</param>
        /// <param name="page">24 images per page</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        [HttpGet("user/{userId}/images")]
        [ProducesResponseType<IEnumerable<UserImageDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<UserImageDto>>> GetUserImages(
            [Range(0, long.MaxValue)] long userId,
            [Range(0, 1000)] int page,
            CancellationToken cancellationToken
        )
        {
            var images = await _querySender.QueryAsync(
                new GetUserImagesQuery(userId, page, User),
                cancellationToken
            );
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
        [HttpGet("images/search")]
        [ProducesResponseType<IEnumerable<SearchedImageDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<SearchedImageDto>>> SearchImages(
            [FromQuery] [MaxLength(5)] long[] tags,
            [Range(0, long.MaxValue)] long categoryId = 0,
            [Range(0, 1000)] int page = 0,
            SearchOrder order = SearchOrder.ByDate,
            CancellationToken cancellationToken = default
        )
        {
            var images = await _querySender.QueryAsync(
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
            [Range(0, long.MaxValue)] long albumId,
            [Range(0, long.MaxValue)] long imageId,
            CancellationToken cancellationToken
        )
        {
            var image = await _querySender.QueryAsync(
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
        /// <param name="albumId">Album that removed images belong to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The removed images</response>
        [Authorize]
        [HttpGet("album/{albumId}/images/removed")]
        [ProducesResponseType<IEnumerable<AlbumImageDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<AlbumImageDto>>> GetRemovedImages(
            [Range(0, long.MaxValue)] long albumId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var images = await _querySender.QueryAsync(
                new GetRemovedImagesQuery(albumId, User),
                cancellationToken
            );
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
            [Range(0, long.MaxValue)] long albumId,
            [Range(0, long.MaxValue)] long imageId
        )
        {
            await _commandSender.CommandAsync(new RemoveImageCommand(albumId, imageId, User));
            return Responses.NoContent;
        }

        /// <summary>
        /// Add Image
        /// </summary>
        /// <remarks>
        /// Add a new image to an album
        /// <para>Authorization is required</para>
        /// </remarks>
        /// <param name="request">The new image info.</param>
        /// <param name="albumId">Album that the image to be added belongs to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="201">The image is added successfully</response>
        [Authorize]
        [HttpPost("album/{albumId}/add")]
        [ProducesResponseType<ImageInfoDto>(StatusCodes.Status201Created)]
        public async Task<Created<ImageInfoDto>> AddImage(
            [FromForm] AddImageRequest request,
            [FromRoute] [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
        )
        {
            var command = new AddImageCommand(
                request.Title,
                request.Description,
                request.Tags,
                request.Image,
                albumId,
                User
            );
            var response = await _commandSender.CommandAsync(command, cancellationToken);
            return Responses.Created(response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        [HttpGet("image/{imageId}")]
        [Produces("image/png", "image/jpeg", "image/gif", "image/webp", "image/svg+xml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> GetImageFile(
            [FromRoute] long imageId,
            [FromQuery] bool thumbnail = false,
            CancellationToken cancellationToken = default
        )
        {
            // file will be disposes automatically after return;
            var file = await _querySender.QueryAsync(
                new GetImageFileQuery(imageId, thumbnail, User),
                cancellationToken
            );

            if (file is null)
            {
                return Results.NotFound();
            }

            return Results.File(file, "image/*");
        }
    }
}
