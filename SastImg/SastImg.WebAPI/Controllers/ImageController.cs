using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.RemoveImage;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.WebAPI.Requests.ImageRequest;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    [Route("api")]
    [ApiController]
    public sealed class ImageController(
        IQueryRequestSender querySender,
        ICommandRequestSender commandSender
    ) : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("album/{albumId}/images")]
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
        /// TODO: complete
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("images/search")]
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
        /// TODO: complete
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("album/{albumId}/image/{imageId}")]
        public async Task<Results<Ok<DetailedImageDto>, NotFound>> GetImage(
            [Range(0, long.MaxValue)] long albumId,
            [Range(0, long.MaxValue)] long imageId,
            CancellationToken cancellationToken
        )
        {
            var image = await _querySender.QueryAsync(
                new GetImageQuery(imageId, User),
                cancellationToken
            );
            return Responses.DataOrNotFound(image);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("album/{albumId}/images/removed")]
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
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("album/{albumId}/image/{imageId}/remove")]
        public async Task<NoContent> RemoveImage(
            [Range(0, long.MaxValue)] long albumId,
            [Range(0, long.MaxValue)] long imageId
        )
        {
            await _commandSender.CommandAsync(new RemoveImageCommand(albumId, imageId, User));
            return Responses.NoContent;
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="request"></param>
        /// <param name="albumId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("album/{albumId}/add")]
        public async Task<Created<ImageInfoDto>> AddImageAsync(
            [FromForm] AddImageRequest request,
            [FromRoute] [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken = default
        )
        {
            using var command = new AddImageCommand(
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
    }
}
