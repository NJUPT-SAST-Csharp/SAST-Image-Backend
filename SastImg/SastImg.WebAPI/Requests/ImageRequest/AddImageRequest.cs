using System.ComponentModel.DataAnnotations;
using SastImg.WebAPI.Validators;

namespace SastImg.WebAPI.Requests.ImageRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public readonly struct AddImageRequest
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        [FileValidator(50)]
        public readonly IFormFile Image { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(30)]
        public readonly string Title { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(100)]
        public readonly string Description { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        public readonly long[] Tags { get; init; }
    }
}
