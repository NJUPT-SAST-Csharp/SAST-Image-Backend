using System.ComponentModel.DataAnnotations;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.WebAPI.Requests.AlbumRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public readonly struct CreateAlbumRequest
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(20)]
        public readonly string Title { get; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(100)]
        public readonly string Description { get; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [Range(0, long.MaxValue)]
        public readonly long CategoryId { get; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        public readonly Accessibility Accessibility { get; }
    }
}
