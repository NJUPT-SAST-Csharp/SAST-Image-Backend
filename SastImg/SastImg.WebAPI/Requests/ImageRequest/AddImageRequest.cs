using System.ComponentModel.DataAnnotations;
using Utilities.Validators;

namespace SastImg.WebAPI.Requests.ImageRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public sealed class AddImageRequest
    {
        /// <summary>
        /// Image file.
        /// </summary>
        [FileValidator(50)]
        public IFormFile Image { get; init; }

        /// <summary>
        /// Title of the image.
        /// </summary>
        [MaxLength(30)]
        public string Title { get; init; }

        /// <summary>
        /// Description of the image.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; init; }

        /// <summary>
        /// Tags used to categorize the image.
        /// </summary>
        public long[] Tags { get; init; }
    }
}
