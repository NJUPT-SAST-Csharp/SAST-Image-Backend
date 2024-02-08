using System.ComponentModel.DataAnnotations;

namespace SastImg.WebAPI.Requests.TagRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public readonly struct CreateTagRequest
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(10)]
        public readonly string Name { get; init; }
    }
}
