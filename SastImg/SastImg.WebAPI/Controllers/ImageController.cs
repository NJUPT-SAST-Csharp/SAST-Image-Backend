using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Request;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.SearchImages;
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
        ICommandSender commandSender
    ) : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandSender _commandSender = commandSender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("images/{albumId}")]
        public async Task<Ok<IEnumerable<ImageDto>>> GetImages(
            [Range(0, long.MaxValue)] long albumId = 0,
            [Range(0, 1000)] int page = 0,
            CancellationToken cancellationToken = default
        )
        {
            var images = await _querySender.QueryAsync(
                new GetImagesQueryRequest(albumId, page, User),
                cancellationToken
            );

            return Responses.Data(images);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("images")]
        public async Task<Ok<IEnumerable<ImageDto>>> SearchImages(
            [MaxLength(5)] [Range(0, long.MaxValue)] long[] tags,
            [Range(0, long.MaxValue)] long categoryId = 0,
            [Range(0, 1000)] int page = 0,
            SearchOrder order = SearchOrder.ByDate,
            CancellationToken cancellationToken = default
        )
        {
            var images = await _querySender.QueryAsync(
                new SearchImagesQueryRequest(page, order, categoryId, tags, User),
                cancellationToken
            );
            return Responses.Data(images);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("image/{imageId}")]
        public async Task<Results<Ok<DetailedImageDto>, NotFound>> GetImage(
            [Range(0, long.MaxValue)] long imageId,
            CancellationToken cancellationToken
        )
        {
            var image = await _querySender.QueryAsync(
                new GetImageQueryRequest(imageId, User),
                cancellationToken
            );
            return Responses.DataOrNotFound(image);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("images/removed")]
        public async Task<Ok<IEnumerable<ImageDto>>> GetRemovedImages(
            [Range(0, long.MaxValue)] long authorId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var images = await _querySender.QueryAsync(
                new GetRemovedImagesQueryRequest(authorId, User),
                cancellationToken
            );
            return Responses.Data(images);
        }
    }
}
