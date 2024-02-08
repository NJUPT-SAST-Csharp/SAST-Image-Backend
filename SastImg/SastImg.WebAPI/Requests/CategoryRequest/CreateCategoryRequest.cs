using System.ComponentModel.DataAnnotations;

namespace SastImg.WebAPI.Requests.CategoryRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public readonly struct CreateCategoryRequest
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(16)]
        public readonly string Name { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(100)]
        public readonly string Description { get; init; }
    }
}
