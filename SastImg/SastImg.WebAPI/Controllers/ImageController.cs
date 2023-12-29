using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.ImageServices.GetDetailedImage;
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Ok<IEnumerable<ImageDto>>> GetImages(
            [Range(0, long.MaxValue)] long albumId,
            CancellationToken cancellationToken
        )
        {
            var images = await _sender.Send(new GetImagesQueryRequest(albumId), cancellationToken);
            return Responses.Data(images);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{imageId}")]
        public async Task<Results<Ok<DetailedImageDto>, NotFound>> GetImage(
            [Range(0, long.MaxValue)] long imageId,
            CancellationToken cancellationToken
        )
        {
            var image = await _sender.Send(new GetImageQueryRequest(imageId), cancellationToken);
            return Responses.DataOrNotFound(image);
        }
    }
}
