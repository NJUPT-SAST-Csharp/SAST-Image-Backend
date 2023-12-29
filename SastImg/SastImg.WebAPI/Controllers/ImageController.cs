using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ImageController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="page"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Ok<IEnumerable<ImageDto>>> GetImages(
            CancellationToken cancellationToken,
            [Range(0, long.MaxValue)] long albumId = 0,
            [Range(0, 1000)] int page = 0
        )
        {
            var images = await _sender.Send(
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
        [HttpGet("search")]
        public async Task<Ok<IEnumerable<ImageDto>>> SearchImages()
        {
            // TODO: implement
            return Responses.Data<IEnumerable<ImageDto>>([]);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{imageId}")]
        public async Task<Results<Ok<DetailedImageDto>, NotFound>> GetImage(
            CancellationToken cancellationToken,
            [Range(0, long.MaxValue)] long imageId
        )
        {
            var image = await _sender.Send(
                new GetImageQueryRequest(imageId, User),
                cancellationToken
            );
            return Responses.DataOrNotFound(image);
        }
    }
}
