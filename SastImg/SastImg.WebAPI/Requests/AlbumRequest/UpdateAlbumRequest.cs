using System.ComponentModel.DataAnnotations;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.WebAPI.Requests.AlbumRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public readonly struct UpdateAlbumRequest
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(20)]
        public readonly string Title { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(100)]
        public readonly string Description { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        [Range(0, long.MaxValue)]
        public readonly long CategoryId { get; init; }

        /// <summary>
        /// TODO: complete
        /// </summary>
        public Accessibility Accessibility { get; init; }
    }
}
