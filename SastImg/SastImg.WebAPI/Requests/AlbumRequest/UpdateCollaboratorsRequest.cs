using System.ComponentModel.DataAnnotations;

namespace SastImg.WebAPI.Requests.AlbumRequest
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public readonly struct UpdateCollaboratorsRequest
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        [MaxLength(5)]
        public long[] Collaborators { get; init; }
    }
}
